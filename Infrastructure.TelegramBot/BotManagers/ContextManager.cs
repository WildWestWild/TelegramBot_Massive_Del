using Infrastructure.Storage.DbContext;
using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.TelegramBot.BotManagers;

public class ContextManager
{
    private readonly IDbContext _db;
    private readonly ILogger<ContextManager> _logger;

    public ContextManager(IDbContext db, ILogger<ContextManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    public Task<UserContext?> GetContext(long chatId, CancellationToken token)
    {
        return _db.UserContexts.SingleOrDefaultAsync(r => r.ChatId.Equals(chatId), cancellationToken: token);
    }
    
    public UserContext GetContextByCache(long chatId) => _db.UserContexts.Local.Single(r => r.ChatId.Equals(chatId));

    public async Task CreateContext(long chatId, CommandType? commandType, string? listName, CancellationToken token)
    {
        try
        {
            UserContext context = new UserContext
            {
                ChatId = chatId,
                Command = ConvertCommandType(commandType),
                ListName = listName
            };

            var oldContext = await _db.UserContexts.FirstOrDefaultAsync(userContext => userContext.ChatId.Equals(chatId), cancellationToken: token);
            if (oldContext is not null)
            {
                await RemoveContext(oldContext, token);
            }

            _db.UserContexts.Add(context);
            await _db.SaveChangesAsync(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(CreateContext)}] Method has exception!");
        }
    }

    public Task ChangeContext(long chatId, string? listName, CommandType? commandType, CancellationToken token)
    {
        try
        {
            var userContext = GetContextByCache(chatId);
            userContext.Command = ConvertCommandType(commandType);
            userContext.ListName = listName;
            return _db.SaveChangesAsync(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(ChangeContext)}] Method has exception!");
            return Task.CompletedTask;
        }
    }

    public Task RemoveContext(UserContext userContext, CancellationToken token)
    {
        try
        {
            _db.UserContexts.Remove(userContext);
            return _db.SaveChangesAsync(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(RemoveContext)}] Method has exception!");
            return Task.CompletedTask;
        }
    }
    
    private static int? ConvertCommandType(CommandType? commandType)
    {
        return commandType is not null ? (int) commandType : null;
    }
}