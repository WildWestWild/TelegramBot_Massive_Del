namespace Tests.xUnit.TelegramBotTests;

public class TelegramBotDatasets
{
    internal string MessageText { get; private init; }
    internal long ChartId { get; private init; }
    internal string ExpectedMessageText { get; private init; }
    
    public static IEnumerable<object[]> GetOperationsWitList()
    {
        yield return new object[] { new TelegramBotDatasets { MessageText = "Создай новый список", ChartId = 1, ExpectedMessageText = "Введите название списка: " } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Мой тестовый список", ChartId = 1, ExpectedMessageText = "Мой тестовый список" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 1, ExpectedMessageText = "Введите элемент: " } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Это первый элемент!", ChartId = 1, ExpectedMessageText = "Элемент добавлен!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 1, ExpectedMessageText = "Введите элемент: " } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Это второй элемент!", ChartId = 1, ExpectedMessageText = "Элемент добавлен!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Редактируй элемент", ChartId = 1, ExpectedMessageText = "Введите номер элемента и текст в формате - Номер элемента, пробел, текст. Например: (3 Привет,Мир!)" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "1 Это первый элемент (изменено)!", ChartId = 1, ExpectedMessageText = "Элемент изменён!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Удали элемент", ChartId = 1, ExpectedMessageText = "Введите номер элемента:" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "2", ChartId = 1, ExpectedMessageText = "Элемент удалён!" } };
    }
}