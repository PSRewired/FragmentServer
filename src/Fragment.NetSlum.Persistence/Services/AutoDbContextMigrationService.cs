using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Persistence.Services;

/// <summary>
/// A process-blocking hosted service definition that attempts to execute migrations for the given <see cref="DbContext"/>
/// </summary>
/// <typeparam name="TContext">The <see cref="DbContext"/> acted upon</typeparam>
public class AutoDbContextMigrationService<TContext> : IHostedService where TContext : DbContext
{
    private const string EnvVarName = "DOTNET_EF_AUTO_MIGRATE";

    private readonly ILogger<AutoDbContextMigrationService<TContext>> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public AutoDbContextMigrationService(ILogger<AutoDbContextMigrationService<TContext>> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Automatically gather, log and execute all pending migrations on the database.
    /// This service is intentionally blocking to stop requests from running while the migration is in progress
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (!IsAutoMigrationEnabled())
        {
            _logger.LogWarning("Auto migration was disabled via {VariableName} and will not execute", EnvVarName);
            return;
        }

        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        var latestExistingMigration = (await dbContext.Database.GetAppliedMigrationsAsync(cancellationToken))
            .LastOrDefault();

        _logger.LogWarning("Database is on migration version {Version}", latestExistingMigration ?? "None");

        var newMigrations = (await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
        if (newMigrations.Any())
        {
            var migrationsToApply = string.Join(", ", newMigrations);

            _logger.LogWarning("Pending migrations were found. Will run {Num} migrations now! ({PrevVersion} -> {ToExecute})", newMigrations.Count,
                latestExistingMigration, migrationsToApply);

            await dbContext.Database.MigrateAsync(cancellationToken);
            _logger.LogWarning("Database migration complete!");

            return;
        }

        _logger.LogWarning("Database is up to date");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static bool IsAutoMigrationEnabled()
    {
        var shouldDisable = Environment.GetEnvironmentVariable(EnvVarName);

        return shouldDisable == null || Convert.ToBoolean(shouldDisable);
    }
}
