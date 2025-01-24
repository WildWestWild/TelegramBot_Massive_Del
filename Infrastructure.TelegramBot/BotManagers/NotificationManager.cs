using Infrastructure.Storage.DbContext;
using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Extensions;
using Infrastructure.TelegramBot.Notifications;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.TelegramBot.BotManagers;

public class NotificationManager
{
    private readonly IDbContext _db;
    private readonly ITelegramBotClient _botClient;

    private const string ADD_NOTIFICATION_TEMPLATE = @"
<b>Добавлен элемент в: </b> <u>{0}</u>

<b>Текст элемента:</b> {1}

<b>Перейти -> </b> {2}";
    
    private const string UPDATE_NOTIFICATION_TEMPLATE = @"
<b>Изменен элемент в: </b> <u>{0}</u>

<b>Номер элемента: </b> {1} 

<b>Текст элемента:</b> {2}

<b>Перейти -> </b> {3}";
    
    private const string REMOVE_NOTIFICATION_TEMPLATE = @"
<b>Удален элемент из: </b> <u>{0}</u>

<b>Номер элемента: </b> {1} 

<b>Текст элемента:</b> {2}

<b>Перейти -> </b> {3}";
    
    private const string STRIKINGOUT_POSITIVE_NOTIFICATION_TEMPLATE = @"
<b>Вычеркнут элемент из: </b> <u>{0}</u>

<b>Номер элемента: </b> {1} 

<b>Текст элемента:</b> {2}

<b>Перейти -> </b> {3}";
    
    private const string STRIKINGOUT_CANCEL_NOTIFICATION_TEMPLATE = @"
<b>Отменено вычеркивание элемента из: </b> <u>{0}</u>

<b>Номер элемента: </b> {1} 

<b>Текст элемента:</b> {2}     

<b>Перейти -> </b> {3}";
    
    public static readonly string BotLinkWithoutGuidId = "https://t.me/Massive\\_Del\\_bot?start=";

    public NotificationManager(IDbContext db, ITelegramBotClient botClient)
    {
        _db = db;
        _botClient = botClient;
    }

    public async Task SendNotifications(UserContext context, NotificationType notificationType, string notificationData, long? numberOfElement = null)
    {
        var chatIds = await _db.UserListHistories
            .Where(r => r.ListName.Equals(context.ListName) && !r.ChatId.Equals(context.ChatId))
            .Select(r=>r.ChatId)
            .ToArrayAsync();
        
        await Parallel.ForEachAsync(chatIds, (chatId, token) => new ValueTask(
             _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: GenerateMessageForNotification(context, notificationType, notificationData, numberOfElement),
                cancellationToken: token,
                parseMode: ParseMode.Markdown
            )));
    }

    private string GenerateMessageForNotification(UserContext context, NotificationType notificationType, string notificationData, long? numberOfElement = null) =>
        notificationType switch
        {
            NotificationType.Add => string.Format(ADD_NOTIFICATION_TEMPLATE, context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName)), notificationData, context.ListName.GetLink()),
            NotificationType.Update => string.Format(UPDATE_NOTIFICATION_TEMPLATE, context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName)), numberOfElement, notificationData, context.ListName.GetLink()),
            NotificationType.Remove => string.Format(REMOVE_NOTIFICATION_TEMPLATE, context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName)), numberOfElement, notificationData, context.ListName.GetLink()),
            NotificationType.StrikingOut => string.Format(STRIKINGOUT_POSITIVE_NOTIFICATION_TEMPLATE, context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName)), numberOfElement, notificationData, context.ListName.GetLink()),
            NotificationType.CancelStringOut => string.Format(STRIKINGOUT_CANCEL_NOTIFICATION_TEMPLATE, context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName)), numberOfElement, notificationData, context.ListName.GetLink()),
            _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
        };
}