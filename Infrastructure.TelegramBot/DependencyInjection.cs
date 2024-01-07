using Infrastructure.TelegramBot.Options;
using Infrastructure.TelegramBot.WorkerServices;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Infrastructure.TelegramBot
{
    public static class DependencyInjection
    {
        private static string? _pathToBotToken;
        private static string? _pathToSecretToken;
        
        public static void AddTelegramBot(this IServiceCollection services, BotOptions botOptions)
        {
            
            _pathToBotToken = Path.Combine(Directory.GetCurrentDirectory(), botOptions.BotTokenFileName) 
                              ?? throw new ArgumentNullException(nameof(_pathToBotToken));
            
            _pathToSecretToken = Path.Combine(Directory.GetCurrentDirectory(), botOptions.SecretTokenFileName) 
                                 ?? throw new ArgumentNullException(nameof(_pathToSecretToken));
            
            services.AddHttpClient<ITelegramBotClient>("telegram_bot_client")
                    .AddTypedClient<ITelegramBotClient>(client =>
                    {
                        var botToken = File.ReadAllText(_pathToBotToken);
                        return new TelegramBotClient(botToken, client);
                    });
            
            
            services.AddHostedService(provider =>
            {
                var secretToken = File.ReadAllText(_pathToSecretToken);
                var webhookAddress = $"{botOptions.HostAddress}{botOptions.Route}";
                var telegramClient = provider.GetRequiredService<ITelegramBotClient>();
                return new ConfigureWebhookWorkerService(secretToken, webhookAddress, telegramClient);
            });
            
        }
    }
}
