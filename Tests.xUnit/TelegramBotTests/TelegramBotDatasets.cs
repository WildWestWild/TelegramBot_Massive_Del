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
        yield return new object[] { new TelegramBotDatasets { MessageText = "Привет, мир!", ChartId = 1, ExpectedMessageText = "Элемент добавлен!" } };
    }
}