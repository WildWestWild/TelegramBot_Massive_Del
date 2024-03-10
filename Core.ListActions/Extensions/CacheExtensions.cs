using Core.ListActions.ActionCommands;

namespace Core.ListActions.Extensions;

public static class CacheExtensions
{
    public static string GetCommandKey(this ICommandIdentificator identificator) => identificator.Name;
}