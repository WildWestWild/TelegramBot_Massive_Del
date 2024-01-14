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
                entity.HasKey(userListInfo => userListInfo.Id);
                entity.Property(userListInfo => userListInfo.Id).ValueGeneratedOnAdd();
                entity.HasIndex(userListInfo => new { userListInfo.ChatId, userListInfo.Name })
                    .IsUnique();
            });

            // Настройка сущности UserListElements
            modelBuilder.Entity<UserListElement>(entity =>
            {
                entity.HasKey(userListElements => userListElements.Id);
                entity.Property(userListElements => userListElements.Id).ValueGeneratedOnAdd();
                entity.HasOne(e => e.UserListInfo)
                        .WithMany(i => i.UserListElements)
                        .HasForeignKey(e => e.UserListInfoId);
            });
        }
    }
}
