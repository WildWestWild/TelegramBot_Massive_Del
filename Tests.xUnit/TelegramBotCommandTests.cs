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
    
    [Theory]
    [InlineData("/description")]
    public async Task ProcessCommandTest(string messageText)
    {
        var command = await _commandFactory.CreateCommand(messageText, default, _cts.Token);
        await command.Process(default, _cts.Token);
        if (command is BaseCommandWithContext commandWithContext)
        {
            commandWithContext.OnAfterCommandEvent();
        }
    }
}