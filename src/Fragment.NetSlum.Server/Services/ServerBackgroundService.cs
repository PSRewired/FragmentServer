using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.TcpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Server.Services;

/// <summary>
/// This background service starts available TCP servers registered with the service container.
/// </summary>
public class ServerBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ServerBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var startTasks = new List<Task>();

        var servers = scope.ServiceProvider.GetServices<ITcpServer>();
        foreach (var server in servers)
        {
            startTasks.Add(Task.Run(server.Start, stoppingToken));
        }

        var database = scope.ServiceProvider.GetRequiredService<FragmentContext>();
        var chatLobbyStore = scope.ServiceProvider.GetRequiredService<ChatLobbyStore>();

        foreach(var cl in database.DefaultLobbies)
        {
            chatLobbyStore.AddLobby(ChatLobbyModel.FromEntity(cl));
        }

        foreach(var g in database.Guilds)
        {
            chatLobbyStore.AddLobby(new ChatLobbyModel(0, "", g.Id));
        }

        Task.WaitAll(startTasks.ToArray(), stoppingToken);

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var stopTasks = new List<Task>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ServerBackgroundService>>();
        var servers = scope.ServiceProvider.GetServices<ITcpServer>();

        logger.LogWarning("Received signal for shutdown! Stopping all server instances...");

        foreach (var server in servers)
        {
            stopTasks.Add(Task.Run(() =>
            {
                server.Stop();
                logger.LogWarning("[{Name}] stopped!", server.GetType().Name);
            }, cancellationToken));
        }

        Task.WaitAll(stopTasks.ToArray(), cancellationToken);

        return Task.CompletedTask;
    }
}
