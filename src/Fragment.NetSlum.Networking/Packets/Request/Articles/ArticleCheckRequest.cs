using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Articles;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.Articles;

[FragmentPacket(OpCodes.Data, OpCodes.DataNewCheckRequest)]
public class ArticleCheckRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public ArticleCheckRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var hasUnreadArticles = _database.WebNewsArticles
            .Any(a => !_database.WebNewsReadLogs.Any(
                log => log.WebNewsArticleId == a.Id && log.PlayerAccountId == session.PlayerAccountId));

        return Task.FromResult<ICollection<FragmentMessage>>(new[] { new ArticleCheckResponse()
            .ArticlesAvailable(hasUnreadArticles)
            .Build() });
    }
}
