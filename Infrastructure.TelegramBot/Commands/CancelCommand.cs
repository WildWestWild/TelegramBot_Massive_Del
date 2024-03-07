using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class CancelCommand: BaseCommand
{
    private readonly HistoryManager _historyManager;
    public static bool IsNeedToUseCancelCommand(string message) => message.Equals("Отменить действие");
    
    public CancelCommand(ITelegramBotClient botClient, ContextManager contextManager, HistoryManager historyManager) : base(botClient, contextManager)
    {
        _historyManager = historyManager;
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
                if (UserContext.Command is not null && (CommandType) UserContext.Command == CommandType.GetHistory)
                    await _historyManager.RemovePointer(chatId, token);
                
                if (UserContext.ListName is null)
                    await ContextManager.RemoveContext(UserContext, token);
                else
                    await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
                
            };
        }
        
        return base.Process(chatId, token);
    }
}