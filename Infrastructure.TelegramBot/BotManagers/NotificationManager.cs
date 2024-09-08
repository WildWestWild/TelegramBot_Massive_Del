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
                parseMode: ParseMode.Html
            )));
    }

    private string GenerateMessageForNotification(UserContext context, NotificationType notificationType, string notificationData, long? numberOfElement = null) =>
        notificationType switch
        {
            NotificationType.Add => $"В {context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName))} добавлен элемент: \n {notificationData}. \n Ссылка на список: \n {context.ListName.GetLink()}",
            NotificationType.Update => $"В {context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName))} обновлен элемент под номером {numberOfElement}. \n  Текст изменён на: {notificationData}. \n Ссылка на список: \n {context.ListName.GetLink()}",
            NotificationType.Remove => $"В {context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName))} удален элемент под номером {numberOfElement}. \n Текст удаленного элемента: {notificationData}. \n Ссылка на список: \n {context.ListName.GetLink()}",
            NotificationType.StrikingOut =>$"В {context.ListName?.GetOnlyListName() ?? throw new ArgumentException(nameof(context.ListName))} вычеркнут элемент под номером {numberOfElement}. \n Текст удаленного элемента: {notificationData}. \n Ссылка на список: \n {context.ListName.GetLink()}",
            _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
        };
}