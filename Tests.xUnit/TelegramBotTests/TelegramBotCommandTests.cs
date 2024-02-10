using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Infrastructure.TelegramBot.Commands;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.xUnit.TelegramBotTests;

public class TelegramBotCommandTests
{
    private readonly CommandFactory _commandFactory;
    private readonly ILogger<TelegramBotCommandTests> _logger;
    private readonly DbTestContext _context;
    private readonly CancellationTokenSource _cts = new(TimeSpan.FromMinutes(5));

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true, 
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public TelegramBotCommandTests(CommandFactory commandFactory, ILogger<TelegramBotCommandTests> logger, DbTestContext context)
    {
        _commandFactory = commandFactory;
        _logger = logger;
        _context = context;
    }  
    
    public async Task ProcessCommandTest(string commandText, long chatId, string expectedMessageText)
    {
        var command = await _commandFactory.CreateCommand(commandText, chatId, _cts.Token);
        await command.Process(chatId, _cts.Token);
        command.OnAfterCommandEvent();
        Assert.Contains(expectedMessageText, command.Message);
        _logger.LogInformation($"CommandText = [{commandText}], ChatId = [{chatId}], Expected = [{expectedMessageText}], Actual = [{command.Message}]");
        _logger.LogInformation($"UserContext = {JsonSerializer.Serialize(_context.UserContexts.FirstOrDefault(r=>r.ChatId.Equals(chatId)) ?? default, _serializerOptions)}");
        _logger.LogInformation($"UserInfo = {JsonSerializer.Serialize(_context.UserListInfos.FirstOrDefault(r=>r.ChatId.Equals(chatId)) ?? default, _serializerOptions)}");
    }

    [Theory]
    [InlineData("/description", default, "Я - бот, который поможет хранить и редактировать упорядоченные списки")]
    [InlineData("/start", default, "Давайте начнём!")]
    public Task AllCommandWithoutContextTest(string messageText, long chartId, string expectedMessageText) => ProcessCommandTest(messageText, chartId, expectedMessageText);

    [Theory]
    [MemberData(nameof(TelegramBotDatasets.GetOperationsWitList), MemberType = typeof(TelegramBotDatasets))]
    public Task OperationsWithListTest(TelegramBotDatasets telegramBotDatasets) => ProcessCommandTest(telegramBotDatasets.MessageText, telegramBotDatasets.ChartId, telegramBotDatasets.ExpectedMessageText);
}