using Core.ListActions.ActionCommands;
using Infrastructure.Storage;
using Infrastructure.Storage.Models;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions;

public class AddListAction
{
    private readonly IDbContext _db;
    private readonly ILogger<AddListAction> _logger;

    public AddListAction(IDbContext db, ILogger<AddListAction> logger)
    {
        _db = db;
        _logger = logger;
    }

    public bool AddList(AddListCommand command)
    {
        try
        {
            _db.UserListInfos.Add(new UserListInfo
            {
                ChatId = command.ChatId,
                Name = command.Name,
                IsActiveContext = true
            });

            return _db.SaveChanges() > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(AddList)}] Add list failed for command. ChatId = {command.ChatId}, Name = {command.Name}");
            return false;
        }
    }
}