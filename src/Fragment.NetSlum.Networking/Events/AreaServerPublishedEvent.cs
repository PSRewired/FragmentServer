using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Models;

namespace Fragment.NetSlum.Networking.Events;

public class AreaServerPublishedEvent : IEvent
{
    public AreaServerPublishedEvent(AreaServerInformation areaServerInfo)
    {
        AreaServerInfo = areaServerInfo;
    }

    public AreaServerInformation AreaServerInfo { get; }

}
