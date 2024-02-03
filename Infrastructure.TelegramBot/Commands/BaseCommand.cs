using Infrastructure.TelegramBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.TelegramBot.Commands;

public abstract class BaseCommand
{
    private readonly ITelegramBotClient _botClient;

    protected string Message { get; set; }
    protected string? ListName { get; set; }

    public BaseCommand(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public virtual async Task Process(long chatId, CancellationToken token)
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: Message,
            cancellationToken: token,
            replyMarkup: KeyboardHelper.GetKeyboard(ListName));
    }
}