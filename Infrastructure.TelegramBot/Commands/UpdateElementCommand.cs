using System.Text.RegularExpressions;
using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class UpdateElementCommand: BaseCommand
{
    private readonly ReadListAction _readListAction;
    private readonly UpdateElementFromListAction _updateElementFromListAction;

    private readonly Regex _parseUpdateRegex = new("(\\d+)\\s(.*)", RegexOptions.Compiled);

    public UpdateElementCommand(ITelegramBotClient botClient, ContextManager contextManager, ReadListAction readListAction, UpdateElementFromListAction updateElementFromListAction) : base(botClient, contextManager)
    {
        _readListAction = readListAction;
        _updateElementFromListAction = updateElementFromListAction;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));
        
        if (UserContext.Command is null)
        {
            Message = "Введите номер элемента и текст в формате - Номер элемента, пробел, текст. Например: (3 Привет,Мир!)";
            KeyboardMarkup = KeyboardHelper.GetKeyboard();

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.UpdateElement, token);
            };

            await base.Process(chatId, token);
            return;
        }
        
        var command = new AddOrUpdateElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName))
        };

        var match = _parseUpdateRegex.Match(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)));

        if (!match.Success || await CheckValidNumber(match, command, token))
        {
            Message = "Некорректный номер элемента! ";
            KeyboardMarkup = KeyboardHelper.GetKeyboard();

            AddEventToRemoveContext(token);

            await base.Process(chatId, token);
            return;
        }

        command.Number = Convert.ToUInt16(match.Groups[1].Value);
        command.Data = match.Groups[2].Value;

        if (await _updateElementFromListAction.UpdateFromList(command, token))
        {
            Message = "Элемент изменён!";
            KeyboardMarkup = KeyboardHelper.GetKeyboard(UserContext.ListName);

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };
        }
        else
        {
            Message = "Ошибка! Элемент не был изменён.";
            KeyboardMarkup = KeyboardHelper.GetKeyboard();

            AddEventToRemoveContext(token);
        }

        await base.Process(chatId, token);
    }

    private async Task<bool> CheckValidNumber(Match match, ICommandIdentificator commandIdentificator, CancellationToken token)
    {
        try
        {
            var numberOfElement = Convert.ToUInt16(match.Groups[1].Value);
            return numberOfElement < 1 && numberOfElement > await _readListAction.GetCountElements(commandIdentificator, token);
        }
        catch
        {
            return true;
        }
    }
}