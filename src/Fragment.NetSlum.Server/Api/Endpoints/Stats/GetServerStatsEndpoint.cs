using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Stats;

public class GetServerStatsEndpoint : Endpoint<EmptyRequest, ServerStats>
{
    private readonly FragmentContext _database;

    public GetServerStatsEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/stats");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns statistical data about this server instance";
            s.Description = "Returns statistical data about this server instance";
        });
    }

    public override async Task<ServerStats> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return new ServerStats
        {
            TotalBbsPosts = await _database.BbsThreads.CountAsync(ct),
            ActiveGuilds = await _database.Guilds.CountAsync(ct),
            RegisteredAccounts = await _database.PlayerAccounts.CountAsync(ct),
            RegisteredCharacters = await _database.Characters.CountAsync(ct),
        };
    }
}