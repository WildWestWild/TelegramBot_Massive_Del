namespace Tests.xUnit.TelegramBotTests;

public class TelegramBotDatasets
{
    internal string MessageText { get; private init; }
    internal long ChartId { get; private init; }
    internal string ExpectedMessageText { get; private init; }
    
    public static IEnumerable<object[]> GetOperationsWithRandomList()
    {
        yield return new object[] { new TelegramBotDatasets { MessageText = "Создай новый список", ChartId = 1, ExpectedMessageText = "Введите название списка: " } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Мой тестовый список", ChartId = 1, ExpectedMessageText = "Мой тестовый список" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 1, ExpectedMessageText = "Введите элемент (чтобы выйти из режима записи в список, нажмите 'Отменить действие'):" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Это первый элемент!", ChartId = 1, ExpectedMessageText = "Элемент добавлен!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Это второй элемент!", ChartId = 1, ExpectedMessageText = "Элемент добавлен!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Отменить действие", ChartId = 1, ExpectedMessageText = "Действие отменено!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Редактируй элемент", ChartId = 1, ExpectedMessageText = "Введите номер элемента и текст в формате - Номер элемента, пробел, текст. Например: (3 Привет,Мир!)" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "1 Это первый элемент (изменено)!", ChartId = 1, ExpectedMessageText = "Элемент изменён!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Удали элемент", ChartId = 1, ExpectedMessageText = "Введите номер элемента:" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "2", ChartId = 1, ExpectedMessageText = "Элемент удалён!" } };
    }
    
    public static IEnumerable<object[]> GetOperationsWithPreparedList()
    {
        yield return new object[] { new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = $"Данный список не может быть отображен" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 2, ExpectedMessageText = "Введите элемент (чтобы выйти из режима записи в список, нажмите 'Отменить действие'):" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = $"Это первый элемент в списке - '{TelegramBotCommandTests.PreparedListName}'!", ChartId = 2, ExpectedMessageText = "Элемент добавлен!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Отменить действие", ChartId = 2, ExpectedMessageText = "Действие отменено!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = $"__{TelegramBotCommandTests.PreparedListName}__" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Вычеркни элемент", ChartId = 2, ExpectedMessageText = "Введите номер элемента (если хотите отменить вычеркивание, используйте это действия на вычеркнутый элемент): " } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "1", ChartId = 2, ExpectedMessageText = "Элемент вычеркнут!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = @$"1\. ~Это первый элемент в списке \- '{TelegramBotCommandTests.PreparedListName}'\!~ " } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 2, ExpectedMessageText = "Введите элемент (чтобы выйти из режима записи в список, нажмите 'Отменить действие'):" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "_, *, [, ], (, ), ~, `, >, #, +, -, =, |, {, }, ., !", ChartId = 2, ExpectedMessageText = "Элемент добавлен!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = "Отменить действие", ChartId = 2, ExpectedMessageText = "Действие отменено!" } };
        yield return new object[] { new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = @" \_, \*, \[, \], \(, \), \~, \`, \>, \#, \+, \-, \=, \|, \{, \}, \., \!" } };
    }
}