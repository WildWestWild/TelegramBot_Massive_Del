using Infrastructure.Storage;
using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TelegramBot;

public class ContextManager
{
    private readonly IDbContext _db;

    public ContextManager(IDbContext db)
    {
        _db = db;
    }

    public Task<UserContext?> GetContext(long chatId, CancellationToken token)
    {
        _db.Database.EnsureCreated();
        return _db.UserContexts.SingleOrDefaultAsync(r => r.ChatId.Equals(chatId), cancellationToken: token);
    }
    
    public UserContext GetContextByCache(long chatId) => _db.UserContexts.Local.Single(r => r.ChatId.Equals(chatId));

    public async Task CreateContext(long chatId, CommandType? commandType, CancellationToken token)
    {
        UserContext context = new UserContext
        {
            ChatId = chatId,
            Command = ConvertCommandType(commandType)
        };

        if (await _db.UserContexts.AnyAsync(userContext => userContext.ChatId.Equals(chatId),cancellationToken: token))
        {
            await RemoveContext(context, token);
        }

        _db.UserContexts.Add(context);
        await _db.SaveChangesAsync(token);
    }

    public Task ChangeContext(long chatId, CommandType? commandType, CancellationToken token)
    {
        var userContext = GetContextByCache(chatId);
        userContext.Command =  ConvertCommandType(commandType);
        return _db.SaveChangesAsync(token);
    }
    
    public Task ChangeContext(long chatId, string listName, CommandType? commandType, CancellationToken token)
    {
        var userContext = GetContextByCache(chatId);
        userContext.Command = ConvertCommandType(commandType);
        userContext.ListName = listName;
        return _db.SaveChangesAsync(token);
    }

    public Task RemoveContext(UserContext userContext, CancellationToken token)
    {
        _db.UserContexts.Remove(userContext);
        return _db.SaveChangesAsync(token);
    }
    
    private static int? ConvertCommandType(CommandType? commandType)
    {
        return commandType is not null ? (int) commandType : null;
    }
}