using Core.ListActions.ActionCommands;
using Infrastructure.Storage;
using Core.ListActions.DTO;
using Core.ListActions.Extensions;
using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions
{
    public class ReadListAction
    {
        private readonly IDbContext _db;
        private readonly ILogger<ReadListAction> _logger;

        public ReadListAction(IDbContext db, ILogger<ReadListAction> logger)
        {
            _db = db;
            _logger = logger;
        }

        public UserListElementDTO[]? GetList(ICommandIdentificator command)
        {
            try
            {
                var userListInfo = AddUserInfoWithElementsInContext(command);
                
                _logger.LogInformation($"[{nameof(GetList)}] List has existed. ChatId = {command.ChatId}, Name = {command.Name}");
                
                return userListInfo.UserListElements.GetDtos();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, $"[{nameof(GetList)}] List Not Found. ChatId = {command.ChatId}, Name = {command.Name}");
                return null;
            }
        }
        
        internal UserListInfo AddUserInfoInContext(ICommandIdentificator identificator) =>
            _db.UserListInfos
                .First(record => record.ChatId == identificator.ChatId && record.Name == identificator.Name);

        internal UserListInfo AddUserInfoWithElementsInContext(ICommandIdentificator identificator) =>
            _db.UserListInfos
                .Include(
                    join => join.UserListElements.OrderBy(element => element.Number)
                )
                .First(record => record.ChatId.Equals(identificator.ChatId) && record.Name.Equals(identificator.Name));
        
        
    }
}
