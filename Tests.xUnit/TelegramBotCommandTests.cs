using Infrastructure.TelegramBot.Commands;
using Xunit;

namespace Tests.xUnit;

public class TelegramBotCommandTests
{
    private readonly CommandFactory _commandFactory;
    private readonly CancellationTokenSource _cts = new(TimeSpan.FromMinutes(5));

    public TelegramBotCommandTests(CommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }
    
    
    public async Task<BaseCommand> ProcessCommandTest(string messageText, long charId)
    {
        var command = await _commandFactory.CreateCommand(messageText, charId, _cts.Token);
        await command.Process(default, _cts.Token);
        command.OnAfterCommandEvent();

        return command;
    }

    [Theory]
    [InlineData("/description", default, "Я - бот, который поможет хранить и редактировать упорядоченные списки")]
    [InlineData("/start", default, "Давайте начнём!")]
    public async Task AllCommandWithoutContextTest(string messageText, long charId, string expectedMessageText)
    {
        var command = await ProcessCommandTest(messageText, charId);
        Assert.Contains(expectedMessageText, command.Message);
    }
}