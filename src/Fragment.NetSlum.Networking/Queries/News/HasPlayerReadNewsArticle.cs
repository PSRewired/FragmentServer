using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

namespace Fragment.NetSlum.Networking.Queries.News;

public class HasPlayerReadNewsArticle : IQuery<bool>
{
    public int PlayerId { get; }
    public ushort ArticleId { get; }

    public HasPlayerReadNewsArticle(int playerId, ushort articleId)
    {
        PlayerId = playerId;
        ArticleId = articleId;
    }
}
