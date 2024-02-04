namespace Tests.xUnit.TelegramBotTests;

public class TelegramBotDatasets
{
    public string MessageText { get; private init; }
    public long ChartId { get; private init; }
    public string ExpectedMessageText { get; private init; }
    
    public static IEnumerable<object[]> CreateListCommands()
    {
        yield return new object[] { new TelegramBotDatasets { MessageText = "Создай новый список", ChartId = 1, ExpectedMessageText = "Введите название списка: " } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Мой тестовый список", ChartId = 1, ExpectedMessageText = "Мой тестовый список" } };
    }
}