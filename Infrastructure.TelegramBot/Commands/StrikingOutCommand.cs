using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Infrastructure.TelegramBot.Notifications;
using Infrastructure.TelegramBot.Validators;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class StrikingOutCommand: BaseCommand
{
    private readonly StrikingOutElementAction _strikingOutElementAction;
    private readonly CommandValidator _commandValidator;
    private readonly NotificationManager _notificationManager;

    public StrikingOutCommand(
        ITelegramBotClient botClient, 
        StrikingOutElementAction strikingOutElementAction, 
        CommandValidator commandValidator,  
        ContextManager contextManager, 
        NotificationManager notificationManager
        ) : base(botClient, contextManager)
    {
        _strikingOutElementAction = strikingOutElementAction;
        _commandValidator = commandValidator;
        _notificationManager = notificationManager;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));
        
        KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();
        
        if (UserContext.Command is null)
        {
            Message = ConstantHelper.EnterElemInStrikingOutCommand;
            
            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.StrikingOutElement, token);
            };

            await base.Process(chatId, token);
            return;
        }

        var command = new AddOrUpdateElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName))
        };
        
        if (await _commandValidator.CheckInvalidNumber(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)), command, token))
        {
            Message = ConstantHelper.WrongNumberInStrikingOutCommand;
            
            await base.Process(chatId, token);
            return;
        }

        command.Number = Convert.ToUInt16(EnterCommandText);
        string? dataElement;
        if ((dataElement = await _strikingOutElementAction.StrikingOutElement(command, token)) is not null)
        {
            Message = ConstantHelper.StrikingNumberInStrikingOutCommand;

            AfterCommandEvent += async () =>
            {
                await _notificationManager.SendNotifications(UserContext, NotificationType.StrikingOut, dataElement);
            };
        }
        else
        {
            Message = ConstantHelper.ErrorInStrikingOutCommand;
            AddEventToRemoveContext(token);
        }
        
        await base.Process(chatId, token);
        
        _strikingOutElementAction.OnAfterActionEvent(command);
    }
}