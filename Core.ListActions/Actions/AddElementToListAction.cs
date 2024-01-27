using System.Text.Json;
using Core.ListActions.ActionCommands;
using Infrastructure.Storage;
using Infrastructure.Storage.Models;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions;

public class AddElementToListAction
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

    public bool AddElement(AddElementCommand command)
    {
        try
        {
            var userListInfo = _readListAction.AddUserInfoInContext(command);
            _db.UserListElements.Add(new UserListElement
            {
                UserListInfoId = userListInfo.Id,
                Number = command.Number,
                Data = command.Data
            });

            return _db.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(AddElement)}] Fail to add element. Command = {JsonSerializer.Serialize(command)}");
            return false;
        }
    }
}