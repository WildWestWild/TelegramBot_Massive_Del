using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Storage
{
    public interface IDbContext
    {
        public DbSet<UserListInfo> UserListInfos { get; }

        public DbSet<UserListElement> UserListElements { get; }

        public int SaveChanges();
    }
}
