using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Models;

namespace Fragment.NetSlum.Networking.Events;

public class PlayerLeftChatLobbyEvent : IEvent
{
    public ChatLobbyPlayer Player { get; }

    public PlayerLeftChatLobbyEvent(ChatLobbyPlayer player)
    {
        Player = player;
    }
}
