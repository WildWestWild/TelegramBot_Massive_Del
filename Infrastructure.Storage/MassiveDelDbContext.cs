using Infrastructure.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Storage
{
    public class MassiveDelDbContext : DbContext, IDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(LoggerFactory.Create(settings => settings.SetMinimumLevel(LogLevel.Information)))
                    .UseSqlite(@"Data Source=../Infrastructure.Storage/massive_del.db");
            }
        }
        public DbSet<UserListInfo> UserListInfos { get; set; }

        public DbSet<UserListElement> UserListElements { get; set; }
        
        public DbSet<UserContext> UserContexts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserListInfo>(entity =>
            {
                entity.HasKey(userListInfo => userListInfo.Id);
                entity.Property(userListInfo => userListInfo.Id).ValueGeneratedOnAdd();
                entity.HasIndex(userListInfo => new { userListInfo.ChatId, userListInfo.Name })
                    .IsUnique();
            });
            
            modelBuilder.Entity<UserListElement>(entity =>
            {
                entity.HasKey(userListElements => userListElements.Id);
                entity.Property(userListElements => userListElements.Id).ValueGeneratedOnAdd();
                entity.HasOne(e => e.UserListInfo)
                        .WithMany(i => i.UserListElements)
                        .HasForeignKey(e => e.UserListInfoId);
            });
            
            modelBuilder.Entity<UserContext>(entity =>
            {
                entity.HasKey(userContext => userContext.ChatId);
            });
        }
    }
}
