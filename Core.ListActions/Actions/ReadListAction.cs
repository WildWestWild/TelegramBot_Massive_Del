using Core.ListActions.ActionCommands;
using Infrastructure.Storage;
using Core.ListActions.DTO;
using Core.ListActions.Extensions;
using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.Actions
{
    public class ReadListAction: BaseAction
    {
        private readonly IDbContext _db;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ReadListAction> _logger;
        private readonly TimeSpan _cacheTime = TimeSpan.FromMinutes(5);

        public ReadListAction(IDbContext db, IMemoryCache cache, ILogger<ReadListAction> logger)
        {
            _db = db;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> GetListNameByGuidLink(string guidLink)
        {
            var userInfo = await _db.UserListInfos
                .AsNoTracking()
                .FirstAsync(record => EF.Functions.Like(record.Name, $"%{guidLink}"));
            return userInfo.Name;
        }

        public async Task<UserListElementDTO[]?> GetList(ICommandIdentificator command, CancellationToken token)
        {
            try
            {
                if (_cache.TryGetValue(command.GetCommandKey(), out UserListElementDTO[]? dtos))
                {
                    return dtos;
                }
                
                await AddUserInfoWithElementsInContext(command, token);
                
                _logger.LogInformation($"[{nameof(GetList)}] List has existed. ChatId = {command.ChatId}, Name = {command.Name}");

                return ResetCache(command);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, $"[{nameof(GetList)}] List Not Found. ChatId = {command.ChatId}, Name = {command.Name}");
                return null;
            }
        }

        public async Task<int> GetCountElements(ICommandIdentificator command, CancellationToken token)
        {
            var userInfo = await AddUserInfoWithElementsInContext(command, token);
            return userInfo.UserListElements.Count;
        }

        internal Task<UserListInfo> AddUserInfoWithElementsInContext(ICommandIdentificator identificator, CancellationToken token) =>
            _db.UserListInfos
                .Include(
                    join => join.UserListElements.OrderBy(element => element.Number)
                )
                .FirstAsync(record => record.ChatId.Equals(identificator.ChatId) && record.Name.Equals(identificator.Name), cancellationToken: token);

        internal UserListElementDTO[] ResetCache(ICommandIdentificator identificator)
        {
            var dtos = _db.UserListElements.Local.GetDtos();
            _cache.Set(identificator.GetCommandKey(), dtos, _cacheTime);
            return dtos;
        }
    }
}
