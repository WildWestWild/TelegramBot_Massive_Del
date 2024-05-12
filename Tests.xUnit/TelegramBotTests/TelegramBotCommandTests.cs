using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.BotManagers;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.xUnit.TelegramBotTests;

public class TelegramBotCommandTests
{
    private readonly CommandFactory _commandFactory;
    private readonly ILogger<TelegramBotCommandTests> _logger;
    private readonly DbTestContext _context;
    private readonly CancellationTokenSource _cts = new(TimeSpan.FromMinutes(5));
    public static readonly string PreparedListName = "Подготовленный тестовый список";
    public static readonly string PreparedUniqueListName = $"{PreparedListName}-G-293b8f25-d9f4-4dc7-a9fa-65e87890dafd";

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true, 
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public TelegramBotCommandTests(CommandFactory commandFactory, ILogger<TelegramBotCommandTests> logger,
        DbTestContext context)
    {
        _commandFactory = commandFactory;
        _logger = logger;
        _context = context;

        PrepareTestList(2, 2);
    }

    private void PrepareTestList(long id, long charId, string? subName = null)
    {
        if (!_context.UserListInfos.Any(record => record.Id.Equals(id)))
        {
            _context.UserListInfos.Add(new UserListInfo
            { 
                Id = id,
                Name = (subName ?? string.Empty) + PreparedUniqueListName
            });       
            _context.SaveChanges();
        }
    }

    private async Task ProcessCommandTest(string commandText, long chatId, string expectedMessageText)
    {
        _logger.LogInformation($"CommandText = [{commandText}], ChatId = [{chatId}], Expected = [{expectedMessageText}]");
        var command = await _commandFactory.CreateCommand(commandText, chatId, _cts.Token);
        await command.Process(chatId, _cts.Token);
        command.OnAfterCommandEvent();
        _logger.LogInformation($"CommandType = [{command.GetType().Name}], Actual = [{command.Message}]");
        Assert.Contains(expectedMessageText, command.Message);
        _logger.LogInformation($"UserContext = {JsonSerializer.Serialize(_context.UserContexts.FirstOrDefault(r=>r.ChatId.Equals(chatId)) ?? default, _serializerOptions)}");
        _logger.LogInformation($"UserInfo = {JsonSerializer.Serialize(_context.UserListInfos.FirstOrDefault(r=>r.ChatId.Equals(chatId)) ?? default, _serializerOptions)}");
    }
  
    [Theory]
    [InlineData("/description", default, "Я - бот, который поможет хранить и редактировать упорядоченные списки")]
    [InlineData("/start", default, "Давайте начнём!")]
    public Task AllCommandWithoutContextTest(string messageText, long chartId, string expectedMessageText) => ProcessCommandTest(messageText, chartId, expectedMessageText);

    [Theory]
    [MemberData(nameof(TelegramBotDatasets.GetOperationsWithRandomList), MemberType = typeof(TelegramBotDatasets))]
    public Task OperationsWithRandomListTest(TelegramBotDatasets telegramBotDatasets) => ProcessCommandTest(
        telegramBotDatasets.MessageText, telegramBotDatasets.ChartId, telegramBotDatasets.ExpectedMessageText);
    
    [Theory]
    [MemberData(nameof(TelegramBotDatasets.GetOperationsWithPreparedList), MemberType = typeof(TelegramBotDatasets))]
    public Task OperationsWithPreparedListTest(TelegramBotDatasets telegramBotDatasets) => ProcessCommandTest(
        telegramBotDatasets.MessageText, telegramBotDatasets.ChartId, telegramBotDatasets.ExpectedMessageText);
}