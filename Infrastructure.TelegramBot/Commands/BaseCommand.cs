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

    public virtual async Task Process(Update update, CancellationToken token)
    {
        await _botClient.SendTextMessageAsync(
            chatId: update.Message!.Chat.Id,
            text: Message,
            cancellationToken: token,
            replyMarkup: KeyboardHelper.GetKeyboard(ListName));
    }

    public static BaseCommand FindCommand(IServiceProvider provider, string commandName)
    {
        var commandType = commandName switch
        {
            "/description" => typeof(DescriptionCommand),
            _ => typeof(NotFoundCommand)
        };

        return provider.GetService(commandType) as BaseCommand 
               ?? throw new NullReferenceException(nameof(commandType));
    }
}