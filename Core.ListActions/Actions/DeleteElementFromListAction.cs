using Core.ListActions.ActionCommands;
using Infrastructure.Storage.DbContext;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions;

public class DeleteElementFromListAction: BaseAction
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
            var userInfo = await _readListAction.AddUserInfoWithElementsInContext(command, token);

            var elementForDelete = userInfo.UserListElements.First(r => r.Number.Equals(command.Number));
            
            userInfo.UserListElements.Remove(elementForDelete);

            userInfo.CountSymbolsInList -= elementForDelete.Data.Length;

            var elements = userInfo.UserListElements
                .OrderBy(r=>r.Number)
                .ToArray();
            
            for (ushort i = 0; i < elements.Length; i++)
            {
                elements[i].Number = (ushort)(i + 1);
            }
            
            AfterActionEvent += (identificator) =>
            {
                _readListAction.ResetCache(identificator);
            };

            return await _db.SaveChangesAsync(token) > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(DeleteFromList)}] Delete element failed for command. ChatId = {command.ChatId}, Name = {command.Name}, Number = {command.Number}");
            return false;
        }
    }
}