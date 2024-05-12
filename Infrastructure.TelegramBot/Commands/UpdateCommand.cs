using System.Text.RegularExpressions;
using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Infrastructure.TelegramBot.Validators;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class UpdateCommand: BaseCommand
{
    private readonly UpdateElementFromListAction _updateElementFromListAction;
    private readonly CommandValidator _commandValidator;

    private readonly Regex _parseUpdateRegex = new("(\\d+)\\s(.*)", RegexOptions.Compiled);

    public UpdateCommand(
        ITelegramBotClient botClient, 
        ContextManager contextManager,
        UpdateElementFromListAction updateElementFromListAction,
        CommandValidator commandValidator
        ) : base(botClient, contextManager)
    {
        _updateElementFromListAction = updateElementFromListAction;
        _commandValidator = commandValidator;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));

        if (EnterCommandText is null) throw new ArgumentNullException(nameof(EnterCommandText));
        
        var command = new AddOrUpdateElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName))
        };
        
        if (await _commandValidator.CheckMaxCountSymbolsInList(EnterCommandText, command, token))
        {
            Message = "Невозможно добавить элемент, слишком много символов в списке!";

            KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName);
            
            await base.Process(chatId, token);
            return;
        }
        
        if (UserContext.Command is null)
        {
            Message = "Введите номер элемента и текст в формате - Номер элемента, пробел, текст. Например: (3 Привет,Мир!)";
            KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.UpdateElement, token);
            };

            await base.Process(chatId, token);
            return;
        }
        
        KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(UserContext.ListName);

        var match = _parseUpdateRegex.Match(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)));

        if (!match.Success || await CheckInvalidNumber(match, command, token))
        {
            Message = "Некорректный номер элемента! ";

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };

            await base.Process(chatId, token);
            return;
        }

        command.Number = Convert.ToUInt16(match.Groups[1].Value);
        command.Data = match.Groups[2].Value;

        if (await _updateElementFromListAction.UpdateFromList(command, token))
        {
            Message = "Элемент изменён!";

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };
        }
        else
        {
            Message = "Ошибка! Элемент не был изменён.";

            AddEventToRemoveContext(token);
        }

        await base.Process(chatId, token);
        
        _updateElementFromListAction.OnAfterActionEvent(command);
    }

    private Task<bool> CheckInvalidNumber(Match match, ICommandIdentificator commandIdentificator, CancellationToken token)
        => _commandValidator.CheckInvalidNumber(match.Groups[1].Value, commandIdentificator, token);
}