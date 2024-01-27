using Core.ListActions.ActionCommands;
using Infrastructure.Storage;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions;

public class DeleteElementFromListAction
{
    private readonly IDbContext _db;
    private readonly ReadListAction _readListAction;
    private readonly ILogger<DeleteElementFromListAction> _logger;

    public DeleteElementFromListAction(IDbContext db, ReadListAction readListAction, ILogger<DeleteElementFromListAction> logger)
    {
        _db = db;
        _readListAction = readListAction;
        _logger = logger;
    }

    public async Task<bool> DeleteFromList(DeleteElementCommand command, CancellationToken token)
    {
        try
        {
            var userInfo = await _readListAction.AddUserInfoWithElementsInContext(command);
            
            _db.UserListElements.Remove(
                userInfo.UserListElements.First(
                    r=>r.Number.Equals(command.Number))
            );

            return await _db.SaveChangesAsync(token) > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(DeleteFromList)}] Delete element failed for command. ChatId = {command.ChatId}, Name = {command.Name}, Number = {command.Number}");
            return false;
        }
    }
}