using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.TelegramBot.WorkerServices;

public class ConfigureWebhookWorkerService : IHostedService
{
    private readonly string secretToken;
    private readonly string webhookAddress;
    private readonly ITelegramBotClient telegramBotClient;

    public ConfigureWebhookWorkerService(string secretToken, string webhookAddress, ITelegramBotClient telegramBotClient)
    {
        this.secretToken = secretToken;
        this.webhookAddress = webhookAddress;
        this.telegramBotClient = telegramBotClient;
    }

    public Task StartAsync(CancellationToken cancellationToken) =>

        telegramBotClient.SetWebhookAsync(
            url: webhookAddress,
            allowedUpdates: new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery
            },
            secretToken: secretToken,
            cancellationToken: cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken) => telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
}