using Infrastructure.TelegramBot.BotManagers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.TelegramBot.WorkerServices;

public class ConfigureLongPollingWorkerService: BackgroundService
{
    private const string ERROR_CANCEL_MESSAGE = "Произошла ошибка! ";
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConfigureLongPollingWorkerService> _logger;

    public ConfigureLongPollingWorkerService(ITelegramBotClient telegramBotClient, IServiceProvider serviceProvider, ILogger<ConfigureLongPollingWorkerService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        if (update.Message is not { } message)
            return;
           
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;
        using var scope = _serviceProvider.CreateScope();
        var commandFactory = scope.ServiceProvider.GetRequiredService<CommandFactory>();
        
        try
        {
            var command = await commandFactory.CreateCommand(messageText, chatId, token);
            await command.Process(chatId, token);
            command.OnAfterCommandEvent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(HandleUpdateAsync)}] Method has returned exception!");
            var cancelCommand = commandFactory.CreateCancelCommand(message: ERROR_CANCEL_MESSAGE);
            await cancelCommand.Process(chatId, token);
        }
    }
    
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, $"[{nameof(HandleErrorAsync)}] Message = {exception.Message}");
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message
                }
            },
            stoppingToken
        );
        
        return Task.CompletedTask;
    }
}