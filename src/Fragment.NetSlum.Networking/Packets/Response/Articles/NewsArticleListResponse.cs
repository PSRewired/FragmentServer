using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Articles;

public class NewsArticleListResponse : BaseResponse
{
    //TODO: Add model for new articles
    private ICollection<object> _articles = new List<object>();

    public NewsArticleListResponse SetArticleList(ICollection<object> articles)
    {
        _articles = articles;

        return this;
    }

    public NewsArticleListResponse AddArticle(object article)
    {
        _articles.Add(article);

        return this;
    }

    public override FragmentMessage Build()
    {
        ushort articleCount = (ushort)_articles.Count;

        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, articleCount);

        //TODO: loop through article models, build them into packets and append them to the buffer above

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataNewsArticleList,
            Data = buffer,
        };
    }
}
