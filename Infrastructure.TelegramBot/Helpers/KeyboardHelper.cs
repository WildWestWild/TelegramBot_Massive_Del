using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.Helpers;

public static class KeyboardHelper
{
    private const string NEW_LIST_TEXT = "Создай новый список";
    private const string ADD_ELEMENT_TEXT = "Добавь элемент";
    private const string DELETE_ELEMENT_TEXT = "Удали элемент";
    private const string UPDATE_ELEMENT_TEXT = "Редактируй элемент";
    private const string COPY_LINK_TEXT = "Дай ссылку на список";
    private const string CANCEL_TEXT = "Завершить действие";
    private const string MORE_TEXT = "Ещё";

    public static ReplyKeyboardMarkup GetKeyboardForConcreteList(string uniqueListName)
    {
        return new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new($"Покажи список:\n ({uniqueListName})"),
                        new(NEW_LIST_TEXT)
                    },
                    new KeyboardButton[]
                    {
                        new(ADD_ELEMENT_TEXT),
                        new("Вычеркни элемент")
                    },
                    new KeyboardButton[]
                    {
                        new(DELETE_ELEMENT_TEXT),
                        new(UPDATE_ELEMENT_TEXT)
                    },
                    new KeyboardButton[]
                    {
                        new(COPY_LINK_TEXT)
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
                        new(NEW_LIST_TEXT)
                    }
                })
            { ResizeKeyboard = true };
    }
    
    public static ReplyKeyboardMarkup GetCancelKeyboard()
    {
        return new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new(CANCEL_TEXT)
                    }
                })
            { ResizeKeyboard = true };
    }
    
    public static ReplyKeyboardMarkup GetHistoryKeyboard()
    {
        return new ReplyKeyboardMarkup(
                new List<KeyboardButton[]>()
                {
                    new KeyboardButton[]
                    {
                        new(MORE_TEXT)
                    },
                    new KeyboardButton[]
                    {
                        new(CANCEL_TEXT)
                    }
                })
            { ResizeKeyboard = true };
    }
}