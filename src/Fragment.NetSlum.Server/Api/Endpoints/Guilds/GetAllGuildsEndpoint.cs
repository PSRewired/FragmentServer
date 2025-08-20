using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Guilds;

public class GetAllGuildsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class GetAllGuildsEndpoint : Endpoint<GetAllGuildsRequest, PagedResult<GuildInfo>>
{
    private readonly FragmentContext _database;

    public GetAllGuildsEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/guilds");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns information about all available guilds";
            s.Description = "Returns information about all available guilds";
        });
    }

    public override async Task<PagedResult<GuildInfo>> ExecuteAsync(GetAllGuildsRequest req, CancellationToken ct)
    {
        var guilds = _database.Guilds
            .AsNoTracking()
            .Include(g => g.Stats)
            .Include(g => g.Members)
            .OrderBy(g => g.Id)
            .Paginate(req.Page, req.PageSize);

        var guildCount = await _database.Guilds.CountAsync(ct);

        return new PagedResult<GuildInfo>(req.Page, req.PageSize, guildCount, guilds.Select(g => GuildMapper.Map(g)).ToList());
    }
}