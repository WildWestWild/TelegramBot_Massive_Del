﻿using System.Text.Json;
using Core.ListActions.ActionCommands;
using Infrastructure.Storage;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions;

public class UpdateElementFromListAction
{
    private readonly IDbContext _db;
    private readonly ReadListAction _readListAction;
    private readonly ILogger<UpdateElementFromListAction> _logger;

    public UpdateElementFromListAction(IDbContext db, ReadListAction readListAction, ILogger<UpdateElementFromListAction> logger)
    {
        _db = db;
        _readListAction = readListAction;
        _logger = logger;
    }

    public async Task<bool> UpdateFromList(AddOrUpdateElementCommand command, CancellationToken token)
    {
        try
        {
            await _readListAction.AddUserInfoWithElementsInContext(command);
            var element = _db.UserListElements.First(r => r.Number.Equals(command.Number));
            element.Data = command.Data;
            return await _db.SaveChangesAsync(token) > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"[{nameof(UpdateFromList)}] Fail to add update. Command = {JsonSerializer.Serialize(command)}");
            return false;
        }
    }
}