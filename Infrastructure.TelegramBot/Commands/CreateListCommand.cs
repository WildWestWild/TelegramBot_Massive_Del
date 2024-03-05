using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Extensions;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class CreateListCommand : BaseCommand
{
    private readonly AddListAction _addListAction;

    public CreateListCommand(ITelegramBotClient botClient, ContextManager contextManager, AddListAction addListAction) :
        base(botClient, contextManager)
    {
        _addListAction = addListAction;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext is null || UserContext.ListName is not null)
        {
            Message = "Введите название списка: ";
            KeyboardMarkup = KeyboardHelper.GetStartKeyboard();

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
            Message = $"Список с названием '{uniqueListName.GetOnlyListName()}' успешно создан!";
            KeyboardMarkup = KeyboardHelper.GetKeyboardForConcreteList(uniqueListName);

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, uniqueListName, null, token);
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

    private string CreateUniqueListName(string userListName) => $"{userListName}-G-{Guid.NewGuid()}";
}