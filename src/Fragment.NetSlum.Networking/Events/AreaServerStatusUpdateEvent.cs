using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Core.Constants;

namespace Fragment.NetSlum.Networking.Events;

public class AreaServerStatusUpdateEvent : IEvent
{
    public string ServerName;
    public ushort Level;
    public AreaServerStatus Status;
    public AreaServerState State;
    public ushort CurrentPlayerCount;

    public AreaServerStatusUpdateEvent(string serverName, ushort level, AreaServerStatus status, AreaServerState state, ushort currentPlayerCount)
    {
        ServerName = serverName;
        Level = level;
        Status = status;
        State = state;
        CurrentPlayerCount = currentPlayerCount;
    }
}
