using Core.ListActions;
using FakeItEasy;
using Infrastructure.TelegramBot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Xunit.DependencyInjection.Logging;

namespace Tests.xUnit
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddListActions<DbTestContext>();
            
            services.AddTelegramBotCommands();

            ITelegramBotClient fakeTgBot = A.Fake<ITelegramBotClient>();
            services.AddTransient<ITelegramBotClient>(_ => fakeTgBot);
            
            services.AddLogging(lb =>
            {
                lb.AddConsole();
                lb.AddXunitOutput();
            });
        }
    }
}
