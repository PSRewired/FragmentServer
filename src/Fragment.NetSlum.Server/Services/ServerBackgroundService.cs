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
    private readonly FragmentContext _database;
    private readonly ChatLobbyStore _chatLobbyStore;

    public ServerBackgroundService(IServiceScopeFactory scopeFactory,FragmentContext database, ChatLobbyStore chatLobbyStore)
    {
        _scopeFactory = scopeFactory;
        _database = database;
        _chatLobbyStore = chatLobbyStore;
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

        foreach(var cl in _database.ChatLobbies.Where(c=> c.DefaultChannel == true))
        {
            _chatLobbyStore.AddLobby(new ChatLobbyModel() { ChatLobby = cl });
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
