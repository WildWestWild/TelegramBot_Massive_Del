using Core.ListActions.Actions;
using Infrastructure.Storage;
using Infrastructure.Storage.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ListActions;

public static class DependencyInjection
{
    public static void AddListActions<T>(this IServiceCollection serviceCollection, ServiceLifetime lifetime = ServiceLifetime.Scoped) where T: MassiveDelDbContext
    {
        serviceCollection.AddDbContext<IDbContext, T>(lifetime);
        serviceCollection.AddMemoryCache();
        
        serviceCollection.AddTransient<AddListAction>();
        serviceCollection.AddTransient<ReadListAction>();
        serviceCollection.AddTransient<AddElementToListAction>();
        serviceCollection.AddTransient<DeleteElementFromListAction>();
        serviceCollection.AddTransient<UpdateElementFromListAction>();
        serviceCollection.AddTransient<StrikingOutElementAction>();
    }
}