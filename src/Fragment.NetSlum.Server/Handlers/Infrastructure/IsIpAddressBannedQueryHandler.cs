using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;
using Fragment.NetSlum.Networking.Queries.Infrastructure;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Server.Handlers.Infrastructure;

public class IsIpAddressBannedQueryHandler : QueryHandler<IsIpAddressBannedQuery, bool>
{
    private readonly FragmentContext _database;

    public IsIpAddressBannedQueryHandler(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<bool> Handle(IsIpAddressBannedQuery command, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(_database.BannedIps.Any(log => log.IpAddress == command.IpAddress));
    }
}
