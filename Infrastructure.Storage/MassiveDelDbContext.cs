using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Storage
{
    public class MassiveDelDbContext : DbContext, IDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=massive_del.db");
        }

        public DbSet<UserListInfo> UserListInfos { get; set; }

        public DbSet<UserListElement> UserListElements { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка сущности UserListInfo
            modelBuilder.Entity<UserListInfo>(entity =>
            {
                // Установка составного ключа из ChatId и Name
                entity.HasKey(userListInfo => new { userListInfo.ChatId, userListInfo.Name });

                // Установка внешнего ключа для связи с UserListElements
                entity.HasMany(userListInfo => userListInfo.UserListElements)
                    .WithOne(userListElements => userListElements.UserListInfo)
                    .HasForeignKey(userListElements => userListElements.UserListInfoId);
            });

            // Настройка сущности UserListElements
            modelBuilder.Entity<UserListElement>(entity =>
            {
                entity.HasKey(userListElements => userListElements.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
