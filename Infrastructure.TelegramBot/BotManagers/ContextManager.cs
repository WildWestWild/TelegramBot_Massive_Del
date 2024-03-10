using Infrastructure.Storage.DbContext;
using Infrastructure.Storage.Models;
using Infrastructure.TelegramBot.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TelegramBot.BotManagers;

public class ContextManager
{
    private readonly IDbContext _db;

    public ContextManager(IDbContext db)
    {
        _db = db;
    }

    public Task<UserContext?> GetContext(long chatId, CancellationToken token)
    {
        return _db.UserContexts.SingleOrDefaultAsync(r => r.ChatId.Equals(chatId), cancellationToken: token);
    }
    
    public UserContext GetContextByCache(long chatId) => _db.UserContexts.Local.Single(r => r.ChatId.Equals(chatId));

    public async Task CreateContext(long chatId, CommandType? commandType, string? listName, CancellationToken token)
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

    public Task ChangeContext(long chatId, CommandType? commandType, CancellationToken token)
    {
        var userContext = GetContextByCache(chatId);
        userContext.Command =  ConvertCommandType(commandType);
        return _db.SaveChangesAsync(token);
    }
    
    public Task ChangeContext(long chatId, string? listName, CommandType? commandType, CancellationToken token)
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