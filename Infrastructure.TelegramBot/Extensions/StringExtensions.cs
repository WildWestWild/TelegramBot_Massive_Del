using Infrastructure.TelegramBot.Commands;

namespace Infrastructure.TelegramBot.Extensions;

public static class StringExtensions
{
    public static string GetOnlyListName(this string enterCommandText) =>
        ReadCommand.FindLastUniqueListPart.Replace(enterCommandText, string.Empty);
}