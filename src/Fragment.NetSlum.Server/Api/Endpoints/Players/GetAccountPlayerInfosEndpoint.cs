using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Players;

public class GetAccountPlayerInfosRequest
{
    public int AccountId { get; set; }
}

public class GetAccountPlayerInfosEndpoint : Endpoint<GetAccountPlayerInfosRequest, IEnumerable<PlayerInfo>>
{
    private readonly FragmentContext _database;

    public GetAccountPlayerInfosEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/players/account/{accountId}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns character information for all characters associated with an account ID";
            s.Description = "Returns character information for all characters associated with an account ID";
            s.Response<IEnumerable<PlayerInfo>>(200, "Account player information");
            s.Response(204, "Account does not exist or no data could be found");
        });
    }

    public override async Task<IEnumerable<PlayerInfo>> ExecuteAsync(GetAccountPlayerInfosRequest req, CancellationToken ct)
    {
        var players = await _database.Characters
            .AsNoTracking()
            .Include(p => p.CharacterStats)
            .Where(p => p.PlayerAccountId == req.AccountId)
            .ToListAsync(ct);

        return players.Select(player => CharacterMapper.Map(player));
    }
}