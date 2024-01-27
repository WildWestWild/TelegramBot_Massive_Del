using System.Text.Json;
using Core.ListActions.ActionCommands;
using Infrastructure.Storage;
using Infrastructure.Storage.Models;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions;

public class AddElementToListAction: BaseAction
{
    private readonly IDbContext _db;
    private readonly ReadListAction _readListAction;
    private readonly ILogger<AddElementToListAction> _logger;

    public AddElementToListAction(IDbContext db, ReadListAction readListAction, ILogger<AddElementToListAction> logger)
    {
        _db = db;
        _readListAction = readListAction;
        _logger = logger;
    }

    public async Task<bool> AddElement(AddOrUpdateElementCommand command, CancellationToken token)
    {
        try
        {
            var userListInfo = await _readListAction.AddUserInfoWithElementsInContext(command, token);
            _db.UserListElements.Add(new UserListElement
            {
                UserListInfoId = userListInfo.Id,
                Number = command.Number,
                Data = command.Data
            });

            AfterCommandAction += (identificator) =>
            {
                _readListAction.ResetCache(identificator);
            };

            return await _db.SaveChangesAsync(token) > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(AddElement)}] Fail to add element. Command = {JsonSerializer.Serialize(command)}");
            return false;
        }
    }
}