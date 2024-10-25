using System.Collections.Concurrent;
using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Commands.ActionInterfaces;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Infrastructure.TelegramBot.Notifications;
using Infrastructure.TelegramBot.Validators;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class DeleteCommand: BaseCommand, IFinishCommand
{
    private static ConcurrentDictionary<string, List<ushort>> _deleteSaveActions =
        new();
    
    private readonly DeleteElementFromListAction _deleteElementFromListAction;
    private readonly CommandValidator _commandValidator;
    private readonly NotificationManager _notificationManager;
    public override bool IsNeedSetEnterCommandText => true;
    
    public DeleteCommand(
        ITelegramBotClient botClient, 
        ContextManager contextManager, 
        DeleteElementFromListAction deleteElementFromListAction,
        CommandValidator commandValidator,
        NotificationManager notificationManager) : base(botClient, contextManager)
    {
        _deleteElementFromListAction = deleteElementFromListAction;
        _commandValidator = commandValidator;
        _notificationManager = notificationManager;
    }

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));

        KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();
        
        if (UserContext.Command is null)
        {
            Message = ConstantHelper.InputNumberInDeleteListCommand;
            
            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.DeleteElement, token);
            };

            await base.Process(chatId, token);
            return;
        }

        var command = new DeleteElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName))
        };
        
        if (await _commandValidator.CheckInvalidNumber(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)), command, token))
        {
            Message = ConstantHelper.IncorrectNumberInDeleteListCommand;

            AddEventToRemoveContext(token);

            await base.Process(chatId, token);
            return;
        }

        var number = Convert.ToUInt16(EnterCommandText);
        
        if (_deleteSaveActions.TryGetValue(UserContext.ListName, out var value))
        {
            if (!value.Any(r=>r.Equals(number)))
                value.Add(number);
        }
        else
        {
            _deleteSaveActions.TryAdd(UserContext.ListName, new List<ushort> { number });
        }
        
        Message = ConstantHelper.PickNumberInDeleteListCommand;

        await base.Process(chatId, token);
        
        _deleteElementFromListAction.OnAfterActionEvent(command);
    }
    
    public async Task FinishCommandOnCancel(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));
        
        var command = new DeleteElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName)),
            Numbers = _deleteSaveActions[UserContext.ListName].ToArray()
        };
        
        string[]? elementForDeleteData;
        if ((elementForDeleteData = await _deleteElementFromListAction.DeleteFromList(command, token)) is not null)
        {
            Message = ConstantHelper.DoneActionInDeleteListCommand;

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);

                for (var i = 0; i < command.Numbers.Length; i++)
                    await _notificationManager.SendNotifications(UserContext, NotificationType.Remove, elementForDeleteData[i],
                        command.Numbers[i]);
            };
            
            _deleteElementFromListAction.OnAfterActionEvent(command);   
        }
    }
}