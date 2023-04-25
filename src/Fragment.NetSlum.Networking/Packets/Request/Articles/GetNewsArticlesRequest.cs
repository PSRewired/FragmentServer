using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Articles;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Articles;

[FragmentPacket(OpCodes.Data, OpCodes.DataNewsGetMenu)]
public class GetNewsArticlesRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var includeCategories = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        // we are skipping category listing and we will list only the articles , but either way here is the code for it
        // Ignore categories only send the article list
        /*
        if (u == 0)
        {
            ushort count = 1;
            await SendDataPacket(OpCodes.OPCODE_DATA_NEWS_CATEGORYLIST, BitConverter.GetBytes(swap16(count)));

            ushort catID = 1;
            string catName = "Testing Category";
            using MemoryStream memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(BitConverter.GetBytes(swap16(catID)));
            await memoryStream.WriteAsync(encoding.GetBytes(catName + char.MinValue));
            await SendDataPacket(OpCodes.OPCODE_DATA_NEWS_ENTRY_CATEGORY, memoryStream.ToArray());
        }
        */

        var articleListResponse = new NewsArticleListResponse();

        return Task.FromResult<ICollection<FragmentMessage>>(new[] { articleListResponse.Build() });
    }

}
