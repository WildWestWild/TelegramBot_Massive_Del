using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public abstract class BaseCommand
{
    private readonly ITelegramBotClient _botClient;
    
    protected string? ListName { get; set; }
    protected UserContext? UserContext { get; private set; }
    protected event Action? AfterCommandEvent;
    protected readonly ContextManager ContextManager;
    public string Message { get; protected set; }
    
    public BaseCommand(ITelegramBotClient botClient, ContextManager contextManager)
    {
        _botClient = botClient;
        ContextManager = contextManager;
    }
    
    public virtual void OnAfterCommandEvent()
    {
        AfterCommandEvent?.Invoke();
    }

    public virtual async Task Process(long chatId, CancellationToken token)
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: Message,
            cancellationToken: token,
            replyMarkup: KeyboardHelper.GetKeyboard(ListName));
    }
    
    /// <summary>
    /// Контекст может быть null (тоже значение контекста)
    /// </summary>
    /// <param name="context"></param>
    internal void SetContext(UserContext? context) => UserContext = context;
    
    protected void AddEventToRemoveContext(CancellationToken token)
    {
        if (UserContext is not null)
        {
            AfterCommandEvent += async () => { await ContextManager.RemoveContext(UserContext, token); };
        }
    }
}