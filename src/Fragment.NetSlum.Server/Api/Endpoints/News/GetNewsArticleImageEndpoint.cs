using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace Fragment.NetSlum.Server.Api.Endpoints.News;

public class GetNewsArticleImageRequest
{
    public int ArticleId { get; set; }
}

public class GetNewsArticleImageEndpoint : Endpoint<GetNewsArticleImageRequest, Results<FileContentHttpResult, NotFound>>
{
    private readonly FragmentContext _database;
    private readonly ImageConverter _imageConverter;
    private readonly IFusionCache _cache;

    public GetNewsArticleImageEndpoint(FragmentContext database, ImageConverter imageConverter, IFusionCache cache)
    {
        _database = database;
        _imageConverter = imageConverter;
        _cache = cache;
    }

    public override void Configure()
    {
        Get("/news/{articleId}/image.png");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns the image for a news article";
            s.Description = "Returns the image for a news article as PNG";
        });
    }

    public override async Task<Results<FileContentHttpResult, NotFound>> ExecuteAsync(GetNewsArticleImageRequest req, CancellationToken ct)
    {
        var image = await _database.WebNewsArticles
            .FirstOrDefaultAsync(a => a.Id == req.ArticleId, ct);

        if (image?.Image == null)
        {
            return TypedResults.NotFound();
        }

        var cacheKey = $"image_{req.ArticleId}.png";
        ImageInfo? imageInfo = await _cache.GetOrDefaultAsync<ImageInfo>(cacheKey, token: ct);

        if (imageInfo == null)
        {
            imageInfo = _imageConverter.ConvertPng(image.Image);
            await _cache.SetAsync(cacheKey, imageInfo, token: ct);
        }

        return TypedResults.File(imageInfo.ImageData.ToArray(), "image/png");
    }
}
