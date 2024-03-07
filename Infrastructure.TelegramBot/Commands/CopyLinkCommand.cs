using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Extensions;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot; 
namespace Infrastructure.TelegramBot.Commands;

public class CopyLinkCommand: BaseCommand
{
    public static string BotLink { get; } = "*Ссылка на список:*\nhttps://t.me/Massive\\_Del\\_bot?start=";
    
    public CopyLinkCommand(ITelegramBotClient botClient, ContextManager contextManager) : base(botClient, contextManager)
    {
    }

    public override bool IsNeedSetEnterCommandText => false;   

    public override Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));

        Message = UserContext.ListName.GetLink();
        KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName);

        return base.Process(chatId, token);
    }
}