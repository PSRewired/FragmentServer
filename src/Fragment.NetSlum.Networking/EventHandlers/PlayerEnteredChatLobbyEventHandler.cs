using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Events;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

namespace Fragment.NetSlum.Networking.EventHandlers;

public class PlayerEnteredChatLobbyEventHandler : IEventHandler<PlayerEnteredChatLobbyEvent>
{
    public Task Handle(PlayerEnteredChatLobbyEvent eventInfo, CancellationToken cancellationToken)
    {

        return Task.CompletedTask;
    }
}
