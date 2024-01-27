using Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Tests.xUnit;

public class DbTestContext: MassiveDelDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseInMemoryDatabase(databaseName: "MassiveDelDbInMemory")
                .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddConsole()));
        }
    }
}