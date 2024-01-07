using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.Helpers;

public static class KeyboardHelper
{
    public static ReplyKeyboardMarkup GetKeyboard(string? listName = null)
    {
        var firstItemKeyboard = string.IsNullOrEmpty(listName)
            ? new KeyboardButton[]
            {
                new("Создай новый список")
            }
            : new KeyboardButton[]
            {
                new($"Покажи список \n ({listName})"),
                new("Создай новый список")
            };
        
        return new ReplyKeyboardMarkup(
            new List<KeyboardButton[]>()
            {
                firstItemKeyboard,
                new KeyboardButton[]
                {
                    new("Добавь элемент"),
                    new("Вычеркни элемент")
                },
                new KeyboardButton[]
                {
                    new("Удали элемент"),
                    new("Редактируй элемент")
                },
                new KeyboardButton[]
                {
                    new("Скопируй ссылку на список")
                }
            })
        {
            // автоматическое изменение размера клавиатуры, если не стоит true,
            // тогда клавиатура растягивается чуть ли не до луны,
            // проверить можете сами
            ResizeKeyboard = true,
        };
    }
}