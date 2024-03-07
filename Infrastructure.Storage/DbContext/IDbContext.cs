using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Storage.DbContext
{
    public interface IDbContext: IDisposable
    {
        public DatabaseFacade Database { get; }
        public DbSet<UserListInfo> UserListInfos { get; }

        public DbSet<UserListElement> UserListElements { get; }
        
        public DbSet<UserContext> UserContexts { get; }
        
        public DbSet<UserListHistory> UserListHistories { get; }
        
        public DbSet<UserListHistoryPointer> UserListHistoryPointers { get; }

        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
