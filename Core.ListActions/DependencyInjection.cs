using Core.ListActions.Actions;
using Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ListActions;

public static class DependencyInjection
{
    public static void AddListActions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<IDbContext, MassiveDelDbContext>();

        serviceCollection.AddTransient<AddListAction>();
        serviceCollection.AddTransient<ReadListAction>();
    }
}