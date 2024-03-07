using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Infrastructure.TelegramBot.Extensions;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class AddCommand : BaseCommand
{
    private readonly AddElementToListAction _addElementToListAction;

    public AddCommand(ITelegramBotClient botClient, ContextManager contextManager,
        AddElementToListAction addElementToListAction) : base(botClient, contextManager)
    {
        _addElementToListAction = addElementToListAction;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));
        
        KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();

        if (UserContext.Command is null)
        {
            Message = "Введите элемент (чтобы выйти из режима записи в список, нажмите 'Отменить действие'):";

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.AddElement, token);
            };

            await base.Process(chatId, token);
            return;
        }

        if (EnterCommandText is null) throw new ArgumentNullException(nameof(EnterCommandText));

        var command = new AddOrUpdateElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName)),
            Data = EnterCommandText.ChangeSymbolsForMarkdownV2()
        };

        if (await _addElementToListAction.AddElement(command, token))
        {
            Message = "Элемент добавлен!";
        }
        else
        {
            Message = "Ошибка! Элемент не был добавлен.";

            AddEventToRemoveContext(token);
        }

        await base.Process(chatId, token);

        _addElementToListAction.OnAfterActionEvent(command);
    }
}