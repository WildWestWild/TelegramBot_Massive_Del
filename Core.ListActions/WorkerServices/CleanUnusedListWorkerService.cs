using Infrastructure.Storage.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.ListActions.WorkerServices;

public class CleanUnusedListWorkerService: IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CleanUnusedListWorkerService> _logger;
    private readonly TimeSpan _taskDelay = new(24, 0, 0);

    public CleanUnusedListWorkerService(IServiceProvider serviceProvider, ILogger<CleanUnusedListWorkerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            
            var db = scope.ServiceProvider.GetRequiredService<IDbContext>();
            
            var resultCommand = await db.Database.ExecuteSqlRawAsync(GetCleanAllListInfoSqlScript(), cancellationToken: cancellationToken);
            
            if (!resultCommand.Equals(default))
                _logger.LogError($"[{nameof(StartAsync)}] ExecuteSqlRawAsync return code - {resultCommand}");
            
            await Task.Delay(_taskDelay, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private string GetCleanAllListInfoSqlScript()
    {
        return @"
                BEGIN TRANSACTION;

                CREATE TEMPORARY TABLE UnusableListHistories AS
                SELECT H.ListName,
                       H.ChatId

                FROM UserListHistories H
                WHERE DATE(H.LastUseDate) < date('now', '-3 month');

                DELETE FROM UserListHistories

                WHERE EXISTS
                (
                    SELECT 1 
                      FROM UnusableListHistories UH 
                     WHERE UserListHistories.ChatId = UH.ChatId 
                        AND UserListHistories.ListName = UH.ListName
                );

                DELETE FROM UserListElements

                WHERE EXISTS
                (
                  SELECT 1
                  FROM UnusableListHistories UH
                           JOIN UserListInfos I
                                ON I.ChatId = UH.ChatId
                                    AND I.Name = UH.ListName
                  WHERE UserListElements.UserListInfoId = I.Id
                );

                DELETE FROM UserListInfos

                WHERE EXISTS
                (
                    SELECT 1
                      FROM UnusableListHistories UH
                      WHERE UserListInfos.Name = UH.ListName 
                        AND UserListInfos.ChatId = UH.ChatId
                );

                DROP TABLE UnusableListHistories;
                COMMIT;";
    }
}