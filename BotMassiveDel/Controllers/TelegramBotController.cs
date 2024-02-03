using Infrastructure.TelegramBot;
using Infrastructure.TelegramBot.Commands;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace BotMassiveDel.Controllers
{
    public class TelegramBotController : Controller
    {
        private readonly CommandFactory _commandFactory;

        public TelegramBotController(CommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }
        
        [HttpPost("/Update")]
        public async Task GetMessageFromBot(
            [FromBody] Update update,
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

            var command = await _commandFactory.CreateCommand(messageText, chatId, token);
            await command.Process(chatId, token);
            if (command is BaseCommandWithContext commandWithContext)
            {
                commandWithContext.OnAfterCommandEvent();
            }
        }
    }
}