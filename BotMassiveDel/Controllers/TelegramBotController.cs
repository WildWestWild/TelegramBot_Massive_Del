using Infrastructure.TelegramBot.Commands;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace BotMassiveDel.Controllers
{
    public class TelegramBotController : Controller
    {
        private readonly CommandFactory _commandFactory;
        private readonly ILogger<TelegramBotController> _logger;
        private const string ERROR_CANCEL_MESSAGE = "Произошла ошибка! ";

        public TelegramBotController(CommandFactory commandFactory, ILogger<TelegramBotController> logger)
        {
            _commandFactory = commandFactory;
            _logger = logger;
        }
        
        [HttpPost("/Update")]
        public async Task GetMessageFromBot([FromBody] Update update, CancellationToken token)
        {
            if (update.Message is not { } message)
                return;
           
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;
            
            try
            {
                var command = await _commandFactory.CreateCommand(messageText, chatId, token);
                await command.Process(chatId, token);
                command.OnAfterCommandEvent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[{nameof(GetMessageFromBot)}] Method has exception!");
                var cancelCommand = _commandFactory.CreateCancelCommand(message: ERROR_CANCEL_MESSAGE);
                await cancelCommand.Process(chatId, token);
            }
        }
    }
}