using System.Linq;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class NewsMapper
{
    public static partial IQueryable<NewsArticle> MapNewsArticles(this IQueryable<WebNewsArticle> articles);

    [MapProperty(nameof(WebNewsArticle.WebNewsCategoryId), nameof(NewsArticle.CategoryId))]
    [MapperIgnoreSource(nameof(WebNewsArticle.WebNewsCategory))]
    [MapperIgnoreSource(nameof(WebNewsArticle.Image))]
    public static partial NewsArticle Map(WebNewsArticle article);
}
