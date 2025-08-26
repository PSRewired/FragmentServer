using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.News;

public class GetNewsArticlesRequest
{
    public ushort Id { get; set; }
}

public class GetNewsArticlesEndpoint : Endpoint<GetNewsArticlesRequest, IEnumerable<NewsArticle>>
{
    private readonly FragmentContext _database;

    public GetNewsArticlesEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/news");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns news articles";
            s.Description = "Returns news articles";
        });
    }

    public override async Task<IEnumerable<NewsArticle>> ExecuteAsync(GetNewsArticlesRequest req, CancellationToken ct)
    {
        return await _database.WebNewsArticles
            .OrderByDescending(a => a.CreatedAt)
            .MapNewsArticles()
            .ToListAsync(ct);
    }
}