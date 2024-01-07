using Infrastructure.TelegramBot.Helpers;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotMassiveDel.Controllers
{
    public class TelegramBotController : Controller
    {
        [HttpPost("/Update")]
        public async Task GetMessageFromBot([FromBody] Update update, [FromServices] ITelegramBotClient botClient,
            CancellationToken token)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: token,
                replyMarkup: KeyboardHelper.GetKeyboard());
        }
    }
}