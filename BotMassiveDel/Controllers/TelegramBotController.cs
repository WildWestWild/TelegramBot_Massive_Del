using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotMassiveDel.Controllers
{
    public class TelegramBotController: Controller
    {
        [HttpPost("/Update")]
        public async Task GetMessageFromBot([FromBody] Update update, [FromServices] ITelegramBotClient botClient, CancellationToken token)
        {
            if (update == null)
                return;
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            var replyKeyboard = new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
            new KeyboardButton[]
            {
                new("Покажи список \n (Имя списка)"),
                new("Создай новый список"),
            },
            new KeyboardButton[]
            {
                new("Добавь элемент"),
                new("Вычеркни элемент")
            },
            new KeyboardButton[]
            {
                new("Удали элемент"),
                new("Редактируй элемент")
            }
                })
            {
                // автоматическое изменение размера клавиатуры, если не стоит true,
                // тогда клавиатура растягивается чуть ли не до луны,
                // проверить можете сами
                ResizeKeyboard = true,
            };

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: token,
                replyMarkup: replyKeyboard);
        }
    }
}
