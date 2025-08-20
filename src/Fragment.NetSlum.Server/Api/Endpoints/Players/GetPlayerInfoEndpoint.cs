using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Players;

public class GetPlayerInfoRequest
{
    public int CharacterId { get; set; }
}

public class GetPlayerInfoEndpoint : Endpoint<GetPlayerInfoRequest, PlayerInfo?>
{
    private readonly FragmentContext _database;

    public GetPlayerInfoEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/players/{characterId}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns information about the given player ID";
            s.Description = "Returns information about the given player ID";
            s.Response<PlayerInfo?>(200, "Player information");
            s.Response(204, "Player does not exist or no data could be found");
        });
    }

    public override async Task<PlayerInfo?> ExecuteAsync(GetPlayerInfoRequest req, CancellationToken ct)
    {
        var player = await _database.Characters
            .AsNoTracking()
            .Include(p => p.CharacterStats)
            .FirstOrDefaultAsync(p => p.Id == req.CharacterId, ct);

        return player == null ? null : CharacterMapper.Map(player);
    }
}