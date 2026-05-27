using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;

namespace Fragment.NetSlum.Server.Api.Endpoints.News;

public class CreateNewsArticleRequest
{
    public ushort CategoryId { get; set; } = 1;
    public required string Title { get; set; }
    public required string Content { get; set; }
}

public class CreateNewsArticleEndpoint : Endpoint<CreateNewsArticleRequest, NewsArticle>
{
    private readonly FragmentContext _database;

    public CreateNewsArticleEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Post("/news");
        Permissions(nameof(AuthUserPermissions.ManageNews));
        Summary(s =>
        {
            s.Summary = "Creates a news article";
        });
    }

    public override async Task<NewsArticle> ExecuteAsync(CreateNewsArticleRequest req, CancellationToken ct)
    {
        var article = new WebNewsArticle
        {
            WebNewsCategoryId = req.CategoryId,
            Title = req.Title,
            Content = req.Content,
        };

        _database.WebNewsArticles.Add(article);
        await _database.SaveChangesAsync(ct);

        return NewsMapper.Map(article);
    }
}
