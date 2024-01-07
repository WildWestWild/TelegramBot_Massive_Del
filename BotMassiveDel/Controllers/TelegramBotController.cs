using Infrastructure.TelegramBot.Commands;
using Infrastructure.TelegramBot.Helpers;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotMassiveDel.Controllers
{
    public class TelegramBotController : Controller
    {
        [HttpPost("/Update")]
        public async Task GetMessageFromBot(
            [FromBody] Update update,
            [FromServices] IServiceProvider provider,
            CancellationToken token)
        {
            //TODO: Сделать валидацию FluentValidation
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            var command = BaseCommand.FindCommand(provider, messageText);
            await command.Process(update, token);
        }
    }
}