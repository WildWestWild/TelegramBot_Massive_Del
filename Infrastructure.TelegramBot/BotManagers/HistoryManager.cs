using Infrastructure.Storage;
using Infrastructure.Storage.DbContext;
using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TelegramBot.BotManagers;

public class HistoryManager
{
    private const int TAKE_AND_SKIP_COUNT = 6;

    private readonly IDbContext _db;

    public HistoryManager(IDbContext db)
    {
        _db = db;
    }

    public async Task<UserListHistory[]> GetHistory(long chatId, CancellationToken token)
    {
        var query = _db.UserListHistories
            .Where(record => record.ChatId.Equals(chatId))
            .OrderByDescending(date => date.LastUseDate)
            .Take(TAKE_AND_SKIP_COUNT);
        
        var pointer = await _db.UserListHistoryPointers.FirstOrDefaultAsync(
            record => record.ChatId.Equals(chatId), cancellationToken: token
            );
        
        if (pointer is not null)
        {
            query = query.Skip(pointer.OffSet);
            pointer.OffSet += TAKE_AND_SKIP_COUNT;
        }
        else
        {
            pointer = new UserListHistoryPointer
            {
                ChatId = chatId,
                OffSet = TAKE_AND_SKIP_COUNT
            };

            _db.UserListHistoryPointers.Add(pointer);
        }
        
        await _db.SaveChangesAsync(token);
        
        return await query.ToArrayAsync(token);
    }

    public async Task AddOrUpdateHistory(long chatId, string listName, CancellationToken token)
    {
        var userListHistoryRecord = await _db.UserListHistories.FirstOrDefaultAsync(
            record => record.ChatId.Equals(chatId) && record.ListName.Equals(listName), cancellationToken: token
            );

        if (userListHistoryRecord is not null)
        {
            userListHistoryRecord.LastUseDate = DateTime.Now;
        }
        else
        {
            userListHistoryRecord = new UserListHistory
            {
                ChatId = chatId,
                ListName = listName,
                LastUseDate = DateTime.Now
            };

            _db.UserListHistories.Add(userListHistoryRecord);
        }
        
        await _db.SaveChangesAsync(token);
    }

    public async Task RemovePointer(long chatId, CancellationToken token)
    {
        var point = await _db.UserListHistoryPointers.FirstOrDefaultAsync(
            record => record.ChatId.Equals(chatId), token
            );

        if (point is not null)
        {
            _db.UserListHistoryPointers.Remove(point);
            await _db.SaveChangesAsync(token);
        }
    }
}