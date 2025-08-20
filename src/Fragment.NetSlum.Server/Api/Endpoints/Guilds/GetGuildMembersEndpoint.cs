using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Guilds;

public class GetGuildMembersRequest
{
    public int GuildId { get; set; }
}

public class GetGuildMembersEndpoint : Endpoint<GetGuildMembersRequest, IEnumerable<PlayerInfo>>
{
    private readonly FragmentContext _database;

    public GetGuildMembersEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/guilds/{guildId}/members");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns a list of all current members of the given guild ID";
            s.Description = "Returns a list of all current members of the given guild ID";
        });
    }

    public override async Task<IEnumerable<PlayerInfo>> ExecuteAsync(GetGuildMembersRequest req, CancellationToken ct)
    {
        var members = await _database.Characters
            .AsNoTracking()
            .Include(p => p.CharacterStats)
            .Where(p => p.GuildId == req.GuildId)
            .ToListAsync(ct);

        return members.Select(member => CharacterMapper.Map(member));
    }
}