using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Articles;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Networking.Packets.Request.Articles;

[FragmentPacket(MessageType.Data, OpCodes.DataNewsGetMenuRequest)]
public class GetNewsArticlesRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetNewsArticlesRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var categoryId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        var responses = new List<FragmentMessage>();

        var newsCategories = Array.Empty<WebNewsCategory>();

        if (categoryId < 1)
        {
            newsCategories = _database.WebNewsCategories.ToArray();
        }

        IQueryable<WebNewsArticle> newsArticlesQuery = _database.WebNewsArticles
            .OrderByDescending(n => n.CreatedAt);

        // Send back categories and articles related
        if (newsCategories.Any())
        {
            responses.Add(new NewsCategoryCountResponse((ushort)newsCategories.Length).Build());
            responses.AddRange(newsCategories.Select(n => new NewsCategoryEntryResponse()
                .SetCategoryId(n.Id)
                .SetCategoryName(n.CategoryName)
                .Build()
            ));

            newsArticlesQuery = newsArticlesQuery
                .Where(n => n.WebNewsCategoryId == categoryId);
        }


        var newsArticles = newsArticlesQuery
            .Select(n => new
            {
                Article = n,
                HasBeenRead = _database.WebNewsReadLogs.Any(l =>
                    l.PlayerAccountId == session.PlayerAccountId && l.WebNewsArticleId == n.Id),
            })
            .ToArray();

        responses.Add(new NewsArticleCountResponse((ushort)newsArticles.Length).Build());
        responses.AddRange(newsArticles.Select(n => new NewsArticleEntryResponse()
            .SetArticleId(n.Article.Id)
            .SetTitle(n.Article.Title)
            .SetContent(n.Article.Content)
            .SetArticleCreationDate(n.Article.CreatedAt)
            .IsRead(n.HasBeenRead)
            .Build()
        ));

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
