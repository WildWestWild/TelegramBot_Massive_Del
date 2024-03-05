using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.Helpers;

public static class KeyboardHelper
{
    public const string NewListText = "Создай новый список";
    public static readonly string AddElementText = "Добавь элемент";
    public static readonly string DeleteElementText = "Удали элемент";
    public static readonly string UpdateElementText = "Редактируй элемент";
    public static readonly string CopyLinkText = "Скопируй ссылку на список";

    public static ReplyKeyboardMarkup GetKeyboardForConcreteList(string uniqueListName)
    {
        return new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new($"Покажи список:\n ({uniqueListName})"),
                        new(NewListText)
                    },
                    new KeyboardButton[]
                    {
                        new(AddElementText),
                        new("Вычеркни элемент")
                    },
                    new KeyboardButton[]
                    {
                        new(DeleteElementText),
                        new(UpdateElementText)
                    },
                    new KeyboardButton[]
                    {
                        new(CopyLinkText)
                    }
                })
            { ResizeKeyboard = true };
    }

    public static ReplyKeyboardMarkup GetStartKeyboard()
    {
        return new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new(NewListText)
                    }
                })
            { ResizeKeyboard = true };
    }
}