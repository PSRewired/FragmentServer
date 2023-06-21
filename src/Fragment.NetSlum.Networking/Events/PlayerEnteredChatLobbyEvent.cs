using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Models;

namespace Fragment.NetSlum.Networking.Events;

public class PlayerEnteredChatLobbyEvent : IEvent
{
    public ChatLobbyPlayer Player { get; }

    public PlayerEnteredChatLobbyEvent(ChatLobbyPlayer player)
    {
        Player = player;
    }
}
