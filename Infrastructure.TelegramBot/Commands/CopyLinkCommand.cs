using Infrastructure.TelegramBot.CommandManagers;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot; 
namespace Infrastructure.TelegramBot.Commands;

public class CopyLinkCommand: BaseCommand
{
    private const string BOT_LINK = "*Ссылка на список:*\nhttps://t.me/Massive\\_Del\\_bot?start=";
    
    public CopyLinkCommand(ITelegramBotClient botClient, ContextManager contextManager) : base(botClient, contextManager)
    {
    }

    public override bool IsNeedSetEnterCommandText => false;   

    public override Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));

        Message = BOT_LINK + ReadCommand.FindGuidLinkInText.Match(UserContext.ListName).Value;
        KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName);

        return base.Process(chatId, token);
    }
}