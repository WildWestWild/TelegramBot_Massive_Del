using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class CancelCommand: BaseCommand
{
    private readonly ReadCommand _readCommand;
    private readonly HistoryManager _historyManager;
    public static bool IsNeedToUseCancelCommand(string message) => message.Equals("Завершить действие");
    
    public CancelCommand(ITelegramBotClient botClient, ReadCommand readReadCommand, ContextManager contextManager, HistoryManager historyManager) : base(botClient, contextManager)
    {
        _readCommand = readReadCommand;
        _historyManager = historyManager;
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override Task Process(long chatId, CancellationToken token)
    {
        KeyboardMarkup = (UserContext?.ListName != null)
            ? KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName)
            : KeyboardHelper.GetStartKeyboard();
        
        Message += "Действие завершено!";

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

            if (UserContext.ListName is not null)
                return _readCommand.ReadListInternal(chatId, UserContext.ListName, token);
        }
        
        return base.Process(chatId, token);
    }
}