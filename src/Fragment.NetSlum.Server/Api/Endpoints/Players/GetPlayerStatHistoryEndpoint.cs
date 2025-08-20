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

public class GetPlayerStatHistoryRequest
{
    public int CharacterId { get; set; }
}

public class GetPlayerStatHistoryEndpoint : Endpoint<GetPlayerStatHistoryRequest, IEnumerable<PlayerStats>>
{
    private readonly FragmentContext _database;

    public GetPlayerStatHistoryEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/players/{characterId}/stats");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns historical statistics for the given player ID";
            s.Description = "Returns historical statistics for the given player ID";
            s.Response<IEnumerable<PlayerStats>>(200, "Player statistics history");
            s.Response(204, "Player does not exist or no data could be found");
        });
    }

    public override async Task<IEnumerable<PlayerStats>> ExecuteAsync(GetPlayerStatHistoryRequest req, CancellationToken ct)
    {
        var playerStats = await _database.CharacterStatHistory
            .AsNoTracking()
            .Where(p => p.CharacterId == req.CharacterId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

        return playerStats.Select(stats => CharacterMapper.Map(stats));
    }
}