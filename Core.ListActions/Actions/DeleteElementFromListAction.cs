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

    public async Task<string[]?> DeleteFromList(DeleteElementCommand command, CancellationToken token)
    {
        try
        {
            var userInfo = await _readListAction.AddUserInfoWithElementsInContext(command, token);

            var deleteDataElements = new string[command.Numbers?.Length ?? throw new ArgumentNullException(nameof(command.Numbers))];

            for (var i = 0; i < command.Numbers.Length; i++)
            {
                var elementForDelete = userInfo.UserListElements.First(r => r.Number.Equals(command.Numbers[i]));
            
                userInfo.UserListElements.Remove(elementForDelete);
                
                userInfo.CountSymbolsInList -= elementForDelete.Data.Length;

                deleteDataElements[i] = elementForDelete.Data;
            }

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

            return (await _db.SaveChangesAsync(token) > 0) ? deleteDataElements : null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(DeleteFromList)}] Delete element failed for command. ChatId = {command.ChatId}, Name = {command.Name}, Numbers = {string.Join(',', command.Numbers)}");
            return null;
        }
    }
}