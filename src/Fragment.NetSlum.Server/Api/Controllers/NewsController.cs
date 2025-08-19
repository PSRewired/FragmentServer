using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Converters;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly FragmentContext _database;
    private readonly ImageConverter _imageConverter;
    private readonly IFusionCache _cache;

    public NewsController(FragmentContext database, ImageConverter imageConverter, IFusionCache cache)
    {
        _database = database;
        _imageConverter = imageConverter;
        _cache = cache;
    }

    [HttpGet]
    public async Task<IEnumerable<NewsArticle>> GetNewsArticles(ushort id)
    {
        return await _database.WebNewsArticles
            .OrderByDescending(a => a.CreatedAt)
            .MapNewsArticles()
            .ToListAsync(cancellationToken: HttpContext.RequestAborted);

    }

    [HttpGet("{articleId:int}")]
    public async Task<NewsArticle?> GetNewsArticle(int articleId)
    {
        var article = await _database.WebNewsArticles
            .FirstOrDefaultAsync(a => a.Id == (ushort)articleId, cancellationToken: HttpContext.RequestAborted);

        return article == null ? null : NewsMapper.Map(article);
    }

    [HttpGet("{articleId:int}/image.png")]
    public async Task<IActionResult> GetNewsArticleImage(int articleId)
    {
        var image = await _database.WebNewsArticles
            .FirstOrDefaultAsync(a => a.Id == articleId, cancellationToken: HttpContext.RequestAborted);

        if (image?.Image == null)
        {
            return NotFound();
        }

        var cacheKey = $"image_{articleId}.png";

        ImageInfo? imageInfo = await _cache.GetOrDefaultAsync<ImageInfo>(cacheKey, token: HttpContext.RequestAborted);

        if (imageInfo == null)
        {
            imageInfo = _imageConverter.ConvertPng(image.Image);
            await _cache.SetAsync(cacheKey, imageInfo, token: HttpContext.RequestAborted);
        }

        return File(imageInfo.ImageData.ToArray(), "image/png");
    }

}
