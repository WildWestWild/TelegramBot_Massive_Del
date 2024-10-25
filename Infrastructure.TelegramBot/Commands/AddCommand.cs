﻿using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Infrastructure.TelegramBot.Extensions;
using Infrastructure.TelegramBot.Notifications;
using Infrastructure.TelegramBot.Validators;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class AddCommand : BaseCommand
{
    private readonly AddElementToListAction _addElementToListAction;
    private readonly CommandValidator _commandValidator;
    private readonly NotificationManager _notificationManager;

    public AddCommand(ITelegramBotClient botClient, ContextManager contextManager,
        AddElementToListAction addElementToListAction, CommandValidator commandValidator, NotificationManager notificationManager) : base(botClient, contextManager)
    {
        _addElementToListAction = addElementToListAction;
        _commandValidator = commandValidator;
        _notificationManager = notificationManager;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));

        if (EnterCommandText is null) throw new ArgumentNullException(nameof(EnterCommandText));

        var command = new AddOrUpdateElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName)),
            Data = EnterCommandText.ChangeSymbolsForMarkdownV2()
        };
        
        if (await _commandValidator.CheckMaxCountSymbolsInList(EnterCommandText, command, token))
        {
            Message = ConstantHelper.CannotAddElemInAddCommand;

            KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName);
            
            await base.Process(chatId, token);
            return;
        }
        
        KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();

        if (UserContext.Command is null)
        {
            Message = ConstantHelper.InputElemInAddCommand;

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.AddElement, token);
            };

            await base.Process(chatId, token);
            return;
        }

        if (await _addElementToListAction.AddElement(command, token))
        {
            Message = ConstantHelper.SuccessAddElemInAddCommand;
            
            AfterCommandEvent += async () =>
            {
                await _notificationManager.SendNotifications(UserContext, NotificationType.Add, command.Data);
            };
        }
        else
        {
            Message = ConstantHelper.ErrorCommandElemInAddCommand;

            AddEventToRemoveContext(token);
        }

        await base.Process(chatId, token);

        _addElementToListAction.OnAfterActionEvent(command);
    }
}