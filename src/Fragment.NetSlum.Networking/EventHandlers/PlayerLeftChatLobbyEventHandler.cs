using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Events;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Serilog;

namespace Fragment.NetSlum.Networking.EventHandlers;

public class PlayerLeftChatLobbyEventHandler : IEventHandler<PlayerLeftChatLobbyEvent>
{
    public Task Handle(PlayerLeftChatLobbyEvent eventInfo, CancellationToken cancellationToken)
    {
        // Announce to the lobby that the player has left by broadcasting which player index left
        Log.Information("Notifying that player {PlayerName} has left the room", eventInfo.Player.PlayerIndex);
        eventInfo.Player.ChatLobby.NotifyAllExcept(eventInfo.Player,
            new ClientLeftChatLobbyResponse(eventInfo.Player.PlayerIndex).Build());

        return Task.CompletedTask;
    }
}
