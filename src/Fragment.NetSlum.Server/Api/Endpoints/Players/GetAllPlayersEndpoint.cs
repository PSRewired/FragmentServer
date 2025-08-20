using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Players;

public class GetAllPlayersRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? CharacterName { get; set; }
}

public class GetAllPlayersEndpoint : Endpoint<GetAllPlayersRequest, PagedResult<PlayerInfo>>
{
    private readonly FragmentContext _database;

    public GetAllPlayersEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/players");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns information on all players";
            s.Description = "Returns information on all players";
        });
    }

    public override async Task<PagedResult<PlayerInfo>> ExecuteAsync(GetAllPlayersRequest req, CancellationToken ct)
    {
        IQueryable<Character> characters = _database.Characters
            .AsNoTracking()
            .Include(g => g.CharacterStats);

        if (!string.IsNullOrWhiteSpace(req.CharacterName))
        {
            characters = characters.Where(c =>
                EF.Functions.Collate(
                    EF.Functions.Like(c.CharacterName, $"%{req.CharacterName}%"), $"utf8mb4_0900_ai_ci"));
        }

        var characterCount = await characters.CountAsync(ct);

        var results = await characters.Paginate(req.Page, req.PageSize)
            .Select(r => CharacterMapper.Map(r))
            .ToListAsync(ct);

        return new PagedResult<PlayerInfo>(req.Page, req.PageSize, characterCount, results);
    }
}