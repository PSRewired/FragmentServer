using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.News;

public class GetMotdEndpoint : Endpoint<EmptyRequest, MessageOfTheDay>
{
    private readonly FragmentContext _database;

    public GetMotdEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/motd");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Gets the current server message of the day";
            s.Description = "Gets the current server message of the day";
        });
    }

    public override async Task<MessageOfTheDay> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await _database.ServerNews
            .AsNoTracking()
            .Select(m => new MessageOfTheDay(m.Content, m.CreatedAt))
            .FirstAsync(ct);
    }
}
