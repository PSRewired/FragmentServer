using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Server.Services;

/// <summary>
/// This background service initializes all default lobbies at runtime
/// </summary>
public class ChatLobbyBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer? _sessionTimer;

    public ChatLobbyBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _sessionTimer = new Timer(EvaluateLobbyTimeouts, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
        using var scope = _scopeFactory.CreateScope();

        var chatLobbyStore = scope.ServiceProvider.GetRequiredService<ChatLobbyStore>();
        var database = scope.ServiceProvider.GetRequiredService<FragmentContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ChatLobbyBackgroundService>>();

        var lobbies = database.ChatLobbies
            .ToList()
            .Select(l => new ChatLobbyModel((ushort)l.Id, l.ChatLobbyName, l.LobbyType));

        foreach (var lobby in lobbies)
        {
            chatLobbyStore.AddLobby(lobby);
            logger.LogInformation("Initialized default lobby {LobbyName} ({LobbyId})", lobby.LobbyName, lobby.LobbyId);
        }

        return Task.CompletedTask;
    }

    private void EvaluateLobbyTimeouts(object? state)
    {
        using var scope = _scopeFactory.CreateScope();

        var chatLobbyStore = scope.ServiceProvider.GetRequiredService<ChatLobbyStore>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ChatLobbyBackgroundService>>();

        foreach (var lobby in chatLobbyStore.GetLobbiesByType())
        {
            foreach (var player in lobby.GetPlayers())
            {
                if (player.LastStatusUpdateReceivedAt < DateTime.UtcNow.AddSeconds(60))
                {
                    continue;
                }

                // If we have not received a status update from the player in the last 60 seconds, remove them.
                logger.LogInformation(
                    "Player {PlayerName} in lobby {ChatLobbyName} has timed out. Have not received a status update in {Duration} seconds. Removing player",
                    player.PlayerName,
                    lobby.LobbyName,
                    (DateTime.UtcNow - player.LastStatusUpdateReceivedAt).TotalSeconds
                );

                lobby.RemovePlayer(player);
            }
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _sessionTimer?.Dispose();

        return Task.CompletedTask;
    }
}
