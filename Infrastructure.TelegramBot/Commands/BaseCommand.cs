using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.BotManagers;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.Commands;

public abstract class BaseCommand
{
    private readonly ITelegramBotClient _botClient;
    protected UserContext? UserContext { get; private set; }
    protected ReplyKeyboardMarkup? KeyboardMarkup { get; set; }
    protected event Action? AfterCommandEvent;
    protected readonly ContextManager ContextManager;
    protected string? EnterCommandText { get; private set; }
    public string? Message { get; protected set; }
    public ParseMode ParseMode { get; protected set; } = ParseMode.Markdown;
    public abstract bool IsNeedSetEnterCommandText { get; }

    public BaseCommand(ITelegramBotClient botClient, ContextManager contextManager)
    {
        _botClient = botClient;
        ContextManager = contextManager;
    }
    
    public virtual void OnAfterCommandEvent()
    {
        AfterCommandEvent?.Invoke();
    }

    public virtual Task Process(long chatId, CancellationToken token)
    {
        return _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: Message ?? throw new ArgumentNullException(nameof(Message)),
            cancellationToken: token,
            parseMode: ParseMode,
            replyMarkup: KeyboardMarkup ?? throw new ArgumentNullException(nameof(KeyboardMarkup)));
    }
    
    /// <summary>
    /// Контекст может быть null (тоже значение контекста)
    /// </summary>
    /// <param name="context"></param>
    internal void SetContext(UserContext? context) => UserContext = context;

    internal void SetEnterCommandText(string enterCommandText) => EnterCommandText = enterCommandText;

    protected void AddEventToRemoveContext(CancellationToken token)
    {
        if (UserContext is not null)
        {
            AfterCommandEvent += async () => { await ContextManager.RemoveContext(UserContext, token); };
        }
    }
}