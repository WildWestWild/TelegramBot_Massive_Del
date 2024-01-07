using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.Helpers;

public static class KeyboardHelper
{
    public static ReplyKeyboardMarkup GetKeyboard(string? listName = null)
    {
        return string.IsNullOrEmpty(listName)
            ? new ReplyKeyboardMarkup(
                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                        {
                            new("Создай новый список")
                        }
                    })
                { ResizeKeyboard = true }
            : new ReplyKeyboardMarkup(
                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                        {
                            new($"Покажи список \n ({listName})"),
                            new("Создай новый список")
                        },
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
                { ResizeKeyboard = true };
    }
}