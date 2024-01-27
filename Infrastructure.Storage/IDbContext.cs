using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Storage
{
    public interface IDbContext: IDisposable
    {
        public DatabaseFacade Database { get; }
        public DbSet<UserListInfo> UserListInfos { get; }

        public DbSet<UserListElement> UserListElements { get; }

        public int SaveChanges();
    }
}
