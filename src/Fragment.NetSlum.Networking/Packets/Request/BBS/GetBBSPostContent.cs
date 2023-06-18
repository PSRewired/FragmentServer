using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.BBS;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.BBS;

[FragmentPacket(MessageType.Data, OpCodes.DataBbsGetPostContent)]
public class GetBBSPostContent : BaseRequest
{
    private readonly FragmentContext _database;

    public GetBBSPostContent(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var postId = BinaryPrimitives.ReadUInt32BigEndian(request.Data[4..8].Span);

        var post = _database.BbsPostContents
            .First(pc => pc.PostId == postId);

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new BbsPostContentResponse()
                .SetPostId(post.PostId)
                .SetContent(post.Content)
                .Build()
        });
    }
}
