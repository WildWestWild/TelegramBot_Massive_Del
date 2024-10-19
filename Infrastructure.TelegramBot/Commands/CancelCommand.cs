using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Commands.ActionInterfaces;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class CancelCommand: BaseCommand
{
    private readonly ReadCommand _readCommand;
    private readonly HistoryManager _historyManager;
    private readonly CommandFactory _commandFactory;
    public static bool IsNeedToUseCancelCommand(string message) => message.Equals("Завершить действие");
    
    public CancelCommand(ITelegramBotClient botClient, ReadCommand readReadCommand, ContextManager contextManager, HistoryManager historyManager, CommandFactory commandFactory) : base(botClient, contextManager)
    {
        _readCommand = readReadCommand;
        _historyManager = historyManager;
        _commandFactory = commandFactory;
    }

    public override bool IsNeedSetEnterCommandText => false;

    public override async Task Process(long chatId, CancellationToken token)
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
        }
        
        await base.Process(chatId, token);
        
        if (UserContext?.Command is not null)
        {
            var command = _commandFactory.CreateCommand((CommandType) UserContext.Command, UserContext);
            if (command is IFinishCommand finishCommand)
            {
                await finishCommand.FinishCommandOnCancel(chatId, token);
                command.OnAfterCommandEvent();
            }
        }
            
        if (UserContext?.ListName is not null)
            await _readCommand.ReadListInternal(chatId, UserContext.ListName, token);
    }
}