using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.News;

public class GetNewsArticleRequest
{
    public int ArticleId { get; set; }
}

public class GetNewsArticleEndpoint : Endpoint<GetNewsArticleRequest, NewsArticle?>
{
    private readonly FragmentContext _database;

    public GetNewsArticleEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/news/{articleId}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns a specific news article";
            s.Description = "Returns a specific news article by ID";
        });
    }

    public override async Task<NewsArticle?> ExecuteAsync(GetNewsArticleRequest req, CancellationToken ct)
    {
        var article = await _database.WebNewsArticles
            .FirstOrDefaultAsync(a => a.Id == (ushort)req.ArticleId, ct);

        return article == null ? null : NewsMapper.Map(article);
    }
}