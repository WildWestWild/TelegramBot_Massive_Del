using Infrastructure.TelegramBot.Helpers;

namespace Tests.xUnit.TelegramBotTests;

public class TelegramBotDatasets
{
    internal string MessageText { get; private init; }
    internal long ChartId { get; private init; }
    internal string ExpectedMessageText { get; private init; }
    
    public static IEnumerable<object[]> GetOperationsWithRandomList()
    {
        yield return [new TelegramBotDatasets { MessageText = "Создай новый список", ChartId = 1, ExpectedMessageText = ConstantHelper.InputListNameInCreateListCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Мой тестовый список", ChartId = 1, ExpectedMessageText = "Мой тестовый список" }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 1, ExpectedMessageText = ConstantHelper.InputElemInAddCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Это первый элемент!", ChartId = 1, ExpectedMessageText = ConstantHelper.SuccessAddElemInAddCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Это второй элемент!", ChartId = 1, ExpectedMessageText = ConstantHelper.SuccessAddElemInAddCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Завершить действие", ChartId = 1, ExpectedMessageText = ConstantHelper.DoneActionInCancelCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Редактируй элемент", ChartId = 1, ExpectedMessageText = ConstantHelper.InputElemInUpdateCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "1 Это первый элемент (изменено)!", ChartId = 1, ExpectedMessageText = ConstantHelper.IncorrectNumberInUpdateCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Завершить действие", ChartId = 1, ExpectedMessageText = ConstantHelper.DoneActionInCancelCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Удали элемент", ChartId = 1, ExpectedMessageText = ConstantHelper.InputNumberInDeleteListCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "2", ChartId = 1, ExpectedMessageText = ConstantHelper.PickNumberInDeleteListCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Завершить действие", ChartId = 1, ExpectedMessageText = ConstantHelper.DoneActionInCancelCommand }
        ];
    }
    
    public static IEnumerable<object[]> GetOperationsWithPreparedList()
    {
        yield return [new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = ConstantHelper.ItCannotShowInReadCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 2, ExpectedMessageText = ConstantHelper.InputElemInAddCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = $"Это первый элемент в списке - '{TelegramBotCommandTests.PreparedListName}'!", ChartId = 2, ExpectedMessageText = ConstantHelper.SuccessAddElemInAddCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Завершить действие", ChartId = 2, ExpectedMessageText = ConstantHelper.DoneActionInCancelCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = $"__{TelegramBotCommandTests.PreparedListName}__" }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Вычеркни элемент", ChartId = 2, ExpectedMessageText = ConstantHelper.InputNumberInDeleteListCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "1", ChartId = 2, ExpectedMessageText = ConstantHelper.StrikingNumberInStrikingOutCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Завершить действие", ChartId = 2, ExpectedMessageText = ConstantHelper.DoneActionInCancelCommand}
        ];
        yield return [new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = @$"1\. ~Это первый элемент в списке \- '{TelegramBotCommandTests.PreparedListName}'\!~ " }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Добавь элемент", ChartId = 2, ExpectedMessageText = ConstantHelper.InputElemInAddCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "_, *, [, ], (, ), ~, `, >, #, +, -, =, |, {, }, ., !", ChartId = 2, ExpectedMessageText = ConstantHelper.SuccessAddElemInAddCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = "Завершить действие", ChartId = 2, ExpectedMessageText = ConstantHelper.DoneActionInCancelCommand }
        ];
        yield return [new TelegramBotDatasets { MessageText = $"Покажи список \n ({TelegramBotCommandTests.PreparedUniqueListName})", ChartId = 2, ExpectedMessageText = @" \_, \*, \[, \], \(, \), \~, \`, \>, \#, \+, \-, \=, \|, \{, \}, \., \!" }
        ];
        yield return [new TelegramBotDatasets { MessageText = "/gethistory", ChartId = 2, ExpectedMessageText = "Название: " }
        ];
    }
}