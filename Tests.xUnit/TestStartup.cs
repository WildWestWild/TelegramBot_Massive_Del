using Core.ListActions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection.Logging;

namespace Tests.xUnit
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddListActions<DbTestContext>();
            services.AddLogging(lb =>
            {
                lb.AddConsole();
                lb.AddXunitOutput();
            });
        }
    }
}
