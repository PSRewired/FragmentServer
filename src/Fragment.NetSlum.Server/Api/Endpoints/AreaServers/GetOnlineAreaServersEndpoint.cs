using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Fragment.NetSlum.TcpServer;

namespace Fragment.NetSlum.Server.Api.Endpoints.AreaServers;

public class GetOnlineAreaServersEndpoint : Endpoint<EmptyRequest, IEnumerable<AreaServerStatus>>
{
    private readonly ITcpServer _gameServer;

    public GetOnlineAreaServersEndpoint(ITcpServer gameServer)
    {
        _gameServer = gameServer;
    }

    public override void Configure()
    {
        Get("/areaservers");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns information on all online area servers";
            s.Description = "Returns information on all online area servers";
        });
    }

    public override Task<IEnumerable<AreaServerStatus>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var result = new List<AreaServerStatus>();

        foreach (var client in _gameServer.Sessions)
        {
            if (client is not FragmentTcpSession { IsAreaServer: true } session)
            {
                continue;
            }

            if (session.AreaServerInfo == null)
            {
                continue;
            }

            result.Add(AreaServerMapper.Map(session.AreaServerInfo));
        }

        return Task.FromResult<IEnumerable<AreaServerStatus>>(result.OrderBy(a => a.Name));
    }
}
