using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Infrastructure.TelegramBot.Validators;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class StrikingOutCommand: BaseCommand
{
    private readonly StrikingOutElementAction _strikingOutElementAction;
    private readonly CommandValidator _commandValidator;

    public StrikingOutCommand(ITelegramBotClient botClient, StrikingOutElementAction strikingOutElementAction, CommandValidator commandValidator,  ContextManager contextManager) : base(botClient, contextManager)
    {
        _strikingOutElementAction = strikingOutElementAction;
        _commandValidator = commandValidator;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));

        if (UserContext.Command is null)
        {
            Message = "Введите номер элемента (если хотите отменить вычеркивание, используйте это действия на вычеркнутый элемент): ";
            KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.StrikingOutElement, token);
            };

            await base.Process(chatId, token);
            return;
        }
        
        KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName);
        
        var command = new AddOrUpdateElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName))
        };
        
        if (await _commandValidator.CheckValidNumber(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)), command, token))
        {
            Message = "Некорректный номер элемента! ";

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };

            await base.Process(chatId, token);
            return;
        }

        command.Number = Convert.ToUInt16(EnterCommandText);
        
        if (await _strikingOutElementAction.StrikingOutElement(command, token))
        {
            Message = "Элемент вычеркнут!";

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };
        }
        else
        {
            Message = "Ошибка! Элемент не был вычеркнут.";
            AddEventToRemoveContext(token);
        }
        
        await base.Process(chatId, token);
        
        _strikingOutElementAction.OnAfterActionEvent(command);
    }
}