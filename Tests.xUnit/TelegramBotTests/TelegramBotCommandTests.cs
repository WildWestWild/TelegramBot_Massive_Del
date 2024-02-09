using Infrastructure.TelegramBot.Commands;
using Xunit;

namespace Tests.xUnit.TelegramBotTests;

public class TelegramBotCommandTests
{
    private readonly CommandFactory _commandFactory;
    private readonly CancellationTokenSource _cts = new(TimeSpan.FromMinutes(5));

    public TelegramBotCommandTests(CommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }
    
    public async Task ProcessCommandTest(string commandText, long chatId, string expectedMessageText)
    {
        var command = await _commandFactory.CreateCommand(commandText, chatId, _cts.Token);
        await command.Process(chatId, _cts.Token);
        command.OnAfterCommandEvent();
        Assert.Contains(expectedMessageText, command.Message);
    }

    [Theory]
    [InlineData("/description", default, "Я - бот, который поможет хранить и редактировать упорядоченные списки")]
    [InlineData("/start", default, "Давайте начнём!")]
    public Task AllCommandWithoutContextTest(string messageText, long chartId, string expectedMessageText) => ProcessCommandTest(messageText, chartId, expectedMessageText);

    [Theory]
    [MemberData(nameof(TelegramBotDatasets.CreateListCommands), MemberType = typeof(TelegramBotDatasets))]
    public Task CreateListCommands(TelegramBotDatasets telegramBotDatasets) => ProcessCommandTest(telegramBotDatasets.MessageText, telegramBotDatasets.ChartId, telegramBotDatasets.ExpectedMessageText);
}