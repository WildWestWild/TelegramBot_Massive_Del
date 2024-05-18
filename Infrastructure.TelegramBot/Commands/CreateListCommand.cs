using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Extensions;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class CreateListCommand : BaseCommand
{
    private readonly HistoryManager _historyManager;
    private readonly AddListAction _addListAction;

    public CreateListCommand(ITelegramBotClient botClient, ContextManager contextManager, HistoryManager historyManager, AddListAction addListAction) :
        base(botClient, contextManager)
    {
        _historyManager = historyManager;
        _addListAction = addListAction;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext is null || UserContext.ListName is not null)
        {
            Message = "Введите название списка: ";
            KeyboardMarkup = KeyboardHelper.GetCancelKeyboard();

            AfterCommandEvent += async () =>
            {
                await ContextManager.CreateContext(chatId, CommandType.CreateNewList, null, token);
            };

            await base.Process(chatId, token);
            return;
        }

        var uniqueListName = CreateUniqueListName(EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText)));
        var addListCommand = new AddListCommand
        {
            ChatId = chatId,
            Name = uniqueListName
        };

        if (await _addListAction.AddList(addListCommand, token))
        {
            Message = $"Список с названием '{uniqueListName.GetOnlyListName()}' успешно создан! \n(В списке не может быть более 15-ти тысяч символов)";
            KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(uniqueListName);

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, uniqueListName, null, token);
                await _historyManager.AddOrUpdateHistory(chatId, uniqueListName, token);
            };
        }
        else
        {
            Message = $"Ошибка! Список '{EnterCommandText}' не был добавлен. Возможно стоит попробовать ещё раз.";
            KeyboardMarkup = KeyboardHelper.GetStartKeyboard();

            AfterCommandEvent += async () => { await ContextManager.RemoveContext(UserContext, token); };
        }

        await base.Process(chatId, token);
        
        _addListAction.OnAfterActionEvent(addListCommand);
    }

    private string CreateUniqueListName(string userListName) => $"{userListName.ChangeSymbolsForMarkdownV2()}-G-{Guid.NewGuid()}";
}