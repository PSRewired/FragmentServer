using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Guilds;

public class GetGuildInfoRequest
{
    public int GuildId { get; set; }
}

public class GetGuildInfoEndpoint : Endpoint<GetGuildInfoRequest, GuildInfo?>
{
    private readonly FragmentContext _database;

    public GetGuildInfoEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/guilds/{guildId}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns detailed information about the given guild ID";
            s.Description = "Returns detailed information about the given guild ID";
        });
    }

    public override async Task<GuildInfo?> ExecuteAsync(GetGuildInfoRequest req, CancellationToken ct)
    {
        var guild = await _database.Guilds
            .AsNoTracking()
            .Include(g => g.Stats)
            .Include(g => g.Members)
            .FirstOrDefaultAsync(g => g.Id == req.GuildId, ct);

        return guild == null ? null : GuildMapper.Map(guild);
    }
}