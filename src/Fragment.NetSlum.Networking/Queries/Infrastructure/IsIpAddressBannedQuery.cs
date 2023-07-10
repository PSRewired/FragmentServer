using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

namespace Fragment.NetSlum.Networking.Queries.Infrastructure;

public class IsIpAddressBannedQuery : IQuery<bool>
{
    public string IpAddress { get; set; }

    public IsIpAddressBannedQuery(string ipAddress)
    {
        IpAddress = ipAddress;
    }
}
