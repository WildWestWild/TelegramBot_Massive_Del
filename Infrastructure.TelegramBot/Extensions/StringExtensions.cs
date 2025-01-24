using Infrastructure.TelegramBot.BotManagers;
using Infrastructure.TelegramBot.Commands;

namespace Infrastructure.TelegramBot.Extensions;

public static class StringExtensions
{
    public static string GetOnlyListName(this string enterCommandText) =>
        ReadCommand.FindLastUniqueListPart.Replace(enterCommandText, string.Empty);
    
    public static string GetLinkWithDescription(this string listNameWithGuid) => string.Format(CopyLinkCommand.BotLink, listNameWithGuid.GetOnlyListName()) + ReadCommand.FindGuidLinkInText.Match(listNameWithGuid).Value;
    
    public static string GetLink(this string listNameWithGuid) => NotificationManager.BotLinkWithoutGuidId + ReadCommand.FindGuidLinkInText.Match(listNameWithGuid).Value;

    public static string ChangeSymbolsForMarkdownV2(this string source) => source
        .Replace("_", "\\_")
        .Replace("*", "\\*")
        .Replace("[", "\\[")
        .Replace("]", "\\]")
        .Replace("(", "\\(")
        .Replace(")", "\\)")
        .Replace("~", "\\~")
        .Replace("`", "\\`")
        .Replace(">", "\\>")
        .Replace("#", "\\#")
        .Replace("+", "\\+")
        .Replace("-", "\\-")
        .Replace("=", "\\=")
        .Replace("|", "\\|")
        .Replace("{", "\\{")
        .Replace("}", "\\}")
        .Replace(".", "\\.")
        .Replace("!", "\\!");
}