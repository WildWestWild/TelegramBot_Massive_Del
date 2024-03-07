using Core.ListActions.Actions;
using Infrastructure.TelegramBot.Commands;
using Infrastructure.TelegramBot.Options;
using Infrastructure.TelegramBot.Validators;
using Infrastructure.TelegramBot.WorkerServices;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Infrastructure.TelegramBot;

public static class DependencyInjection
{
    private static string? _pathToBotToken;
    private static string? _pathToSecretToken;
        
    public static void AddTelegramBot(this IServiceCollection services, BotOptions botOptions)
    {

        #region Init_Telegram_Bot

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

        #endregion

        #region Init_Commands

        AddTelegramBotCommands(services);

        #endregion
    }

    public static void AddTelegramBotCommands(this IServiceCollection services)
    {
        services.AddTransient<CommandFactory>();
        services.AddTransient<ContextManager>();
        services.AddTransient<CommandValidator>();
            
        services.AddTransient<NotFoundCommand>();
        services.AddTransient<StartCommand>();
        services.AddTransient<DescriptionCommand>();
        services.AddTransient<CreateListCommand>();
        services.AddTransient<AddCommand>();
        services.AddTransient<UpdateCommand>();
        services.AddTransient<DeleteCommand>();
        services.AddTransient<ReadCommand>();
        services.AddTransient<StrikingOutCommand>();
        services.AddTransient<CancelCommand>();
        services.AddTransient<CopyLinkCommand>();
    }
}