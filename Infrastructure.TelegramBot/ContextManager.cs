using Infrastructure.Storage;
using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.TelegramBot;

public class ContextManager
{
    private readonly IDbContext _db;

    public ContextManager(IDbContext db, ILogger<ContextManager> logger)
    {
        _db = db;
    }

    public Task<UserContext?> GetContext(long chatId, CancellationToken token) => _db.UserContexts.SingleOrDefaultAsync(r => r.ChatId.Equals(chatId), cancellationToken: token);
    
    public UserContext GetContextByCache(long chatId) => _db.UserContexts.Local.Single(r => r.ChatId.Equals(chatId));

    public async Task SaveLastCommandInContext(long chatId, CommandType? commandType, CancellationToken token)
    {
        var userContext = GetContextByCache(chatId);
        userContext.Command = commandType as int?;
        await _db.SaveChangesAsync(token);
    }

    public Task RemoveContext(UserContext userContext, CancellationToken token)
    {
        _db.UserContexts.Remove(userContext);
        return _db.SaveChangesAsync(token);
    }
}