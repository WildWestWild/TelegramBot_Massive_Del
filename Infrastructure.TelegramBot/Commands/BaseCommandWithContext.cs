using Infrastructure.Storage.Models;
using Telegram.Bot;

namespace Infrastructure.TelegramBot.Commands;

public class BaseCommandWithContext: BaseCommand
{
    protected UserContext? UserContext { get; private set; }
    
    protected event Action? AfterCommandEvent;

    public BaseCommandWithContext(ITelegramBotClient botClient) : base(botClient)
    {
    }

    /// <summary>
    /// Контекст может быть null (тоже значение контекста)
    /// </summary>
    /// <param name="context"></param>
    internal void SetContext(UserContext? context) => UserContext = context;

    public virtual void OnAfterCommandEvent()
    {
        AfterCommandEvent?.Invoke();
    }
}