using System.Text.Json;
using Core.ListActions.ActionCommands;
using Infrastructure.Storage.DbContext;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions;

public class StrikingOutElementAction: BaseAction
{
    private readonly IDbContext _db;
    private readonly ReadListAction _readListAction;
    private readonly ILogger<StrikingOutElementAction> _logger;

    public StrikingOutElementAction(IDbContext db, ReadListAction readListAction, ILogger<StrikingOutElementAction> logger)
    {
        _db = db;
        _readListAction = readListAction;
        _logger = logger;
    }
    
    public async Task<string?> StrikingOutElement(AddOrUpdateElementCommand command, CancellationToken token)
    {
        try
        {
            await _readListAction.AddUserInfoWithElementsInContext(command, token);
            var element = _db.UserListElements.Local.First(r => r.Number.Equals(command.Number));
            element.IsStrikingOut = !element.IsStrikingOut;
            
            AfterActionEvent += (identificator) =>
            {
                _readListAction.ResetCache(identificator);
            };
            
            
            
            return (await _db.SaveChangesAsync(token) > 0) ? element.Data : null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(StrikingOutElement)}] Fail to striking out. Command = {JsonSerializer.Serialize(command)}");
            return null;
        }
    }
}