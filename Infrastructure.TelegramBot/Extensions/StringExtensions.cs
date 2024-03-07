using Infrastructure.TelegramBot.Commands;

namespace Infrastructure.TelegramBot.Extensions;

public static class StringExtensions
{
    public static string GetOnlyListName(this string enterCommandText) =>
        ReadCommand.FindLastUniqueListPart.Replace(enterCommandText, string.Empty);
    
    public static string GetLink(this string listName) => CopyLinkCommand.BotLink + ReadCommand.FindGuidLinkInText.Match(listName).Value;

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