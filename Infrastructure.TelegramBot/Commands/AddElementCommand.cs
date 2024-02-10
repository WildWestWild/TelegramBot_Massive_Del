using Core.ListActions.ActionCommands;
using Core.ListActions.Actions;
using Infrastructure.TelegramBot.Enums;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class AddElementCommand: BaseCommand
{
    private readonly AddElementToListAction _addElementToListAction;

    public AddElementCommand(ITelegramBotClient botClient, ContextManager contextManager, AddElementToListAction addElementToListAction) : base(botClient, contextManager)
    {
        _addElementToListAction = addElementToListAction;
    }

    public override bool IsNeedSetEnterCommandText => true;

    public override async Task Process(long chatId, CancellationToken token)
    {
        if (UserContext?.ListName is null) throw new ArgumentNullException(nameof(UserContext));
        
        if (UserContext.Command is null)
        {
            Message = "Введите элемент: ";
            KeyboardMarkup = KeyboardHelper.GetKeyboard();

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, CommandType.AddElement, token);
            };

            await base.Process(chatId, token);
            return;
        }

        var command = new AddOrUpdateElementCommand
        {
            ChatId = chatId,
            Name = UserContext.ListName ?? throw new ArgumentNullException(nameof(UserContext.ListName)),
            Data = EnterCommandText ?? throw new ArgumentNullException(nameof(EnterCommandText))
        };

        if (await _addElementToListAction.AddElement(command, token))
        {
            Message = "Элемент добавлен!";
            KeyboardMarkup = KeyboardHelper.GetKeyboard(UserContext.ListName);

            AfterCommandEvent += async () =>
            {
                await ContextManager.ChangeContext(chatId, UserContext.ListName, null, token);
            };
        }
        else
        {
            Message = "Ошибка! Елемент не был добавлен.";
            KeyboardMarkup = KeyboardHelper.GetKeyboard();

            AfterCommandEvent += async () => { await ContextManager.RemoveContext(UserContext, token); };
        }
        
        await base.Process(chatId, token);
    }
}