using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.TcpServer;

namespace Fragment.NetSlum.Server.Api.Endpoints.Players;

public class GetOnlinePlayersEndpoint : Endpoint<EmptyRequest, IEnumerable<Client>>
{
    private readonly ITcpServer _gameServer;

    public GetOnlinePlayersEndpoint(ITcpServer gameServer)
    {
        _gameServer = gameServer;
    }

    public override void Configure()
    {
        Get("/players/online");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns information on all online players";
            s.Description = "Returns information on all online players";
        });
    }

    public override Task<IEnumerable<Client>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var result = new List<Client>();

        foreach (var client in _gameServer.Sessions)
        {
            if (client is not FragmentTcpSession session || session.IsAreaServer)
            {
                continue;
            }

            result.Add(Client.FromSession(session));
        }

        return Task.FromResult<IEnumerable<Client>>(result);
    }
}
