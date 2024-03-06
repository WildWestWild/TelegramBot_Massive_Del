using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Infrastructure.TelegramBot.Validators;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class DeleteCommand: BaseCommand
{
    private readonly DeleteElementFromListAction _deleteElementFromListAction;
    private readonly CommandValidator _commandValidator;
    public override bool IsNeedSetEnterCommandText => true;
    
    public DeleteCommand(
        ITelegramBotClient botClient, 
        ContextManager contextManager, 
        DeleteElementFromListAction deleteElementFromListAction,
        CommandValidator commandValidator) : base(botClient, contextManager)
    {
        _deleteElementFromListAction = deleteElementFromListAction;
        _commandValidator = commandValidator;
    }

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));

        if (UserContext.Command is null)
        {
            Message = "Введите номер элемента: ";
            KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.DeleteElement, token);
            };

            await base.Process(chatId, token);
            return;
        }
        
        KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName);
        
        var command = new DeleteElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName))
        };
        
        if (await _commandValidator.CheckValidNumber(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)), command, token))
        {
            Message = "Некорректный номер элемента! ";

            AddEventToRemoveContext(token);

            await base.Process(chatId, token);
            return;
        }

        command.Number = Convert.ToUInt16(EnterCommandText);

        if (await _deleteElementFromListAction.DeleteFromList(command, token))
        {
            Message = "Элемент удалён!";

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };
        }
        else
        {
            Message = "Ошибка! Элемент не был удалён.";
            AddEventToRemoveContext(token);
        }
        
        await base.Process(chatId, token);
        
        _deleteElementFromListAction.OnAfterActionEvent(command);
    }
}