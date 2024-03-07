using Infrastructure.TelegramBot.CommandManagers;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class CancelCommand: BaseCommand
{
    public static bool IsNeedToUseCancelCommand(string message) => message.Equals("Отменить действие");
    
    public CancelCommand(ITelegramBotClient botClient, ContextManager contextManager) : base(botClient, contextManager)
    {
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override Task Process(long chatId, CancellationToken token)
    {
        KeyboardMarkup = (UserContext?.ListName != null)
            ? KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName)
            : KeyboardHelper.GetStartKeyboard();

        if (EnterCommandText is not null)
        {
            Message = EnterCommandText;
        }

        Message += "Действие отменено!";

        if (UserContext is not null)
        {
            AfterCommandEvent += async () =>
            {
                if (UserContext.ListName is null)
                    await ContextManager.RemoveContext(UserContext, token);
                else
                    await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };
        }
        
        return base.Process(chatId, token);
    }
}