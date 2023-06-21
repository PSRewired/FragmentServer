using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Fragment.NetSlum.Server.Services;

/// <summary>
/// This background service initializes all default lobbies at runtime
/// </summary>
public class ChatLobbyBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ChatLobbyBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var chatLobbyStore = scope.ServiceProvider.GetRequiredService<ChatLobbyStore>();
        var database = scope.ServiceProvider.GetRequiredService<FragmentContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ChatLobbyBackgroundService>>();

        var lobbies = database.DefaultLobbies
            .ToList()
            .Select(l => new ChatLobbyModel((ushort)l.Id, l.DefaultLobbyName));

        foreach (var lobby in lobbies)
        {
            chatLobbyStore.AddLobby(lobby);
            logger.LogInformation("Initialized default lobby {LobbyName} ({LobbyId})", lobby.LobbyName, lobby.LobbyId);
        }

        return Task.CompletedTask;
    }
}
