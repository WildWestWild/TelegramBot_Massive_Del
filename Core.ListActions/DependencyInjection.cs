﻿using Core.ListActions.Actions;
using Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ListActions;

public static class DependencyInjection
{
    public static void AddListActions<T>(this IServiceCollection serviceCollection) where T: MassiveDelDbContext
    {
        serviceCollection.AddDbContext<IDbContext, T>();

        serviceCollection.AddTransient<AddListAction>();
        serviceCollection.AddTransient<ReadListAction>();
        serviceCollection.AddTransient<AddElementToListAction>();
    }
}