using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.BBS;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.BBS;

[FragmentPacket(OpCodes.Data, OpCodes.DataBbsGetThreadDetails)]
public class GetBBSThreadDetailsRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetBBSThreadDetailsRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var threadId = BinaryPrimitives.ReadUInt32BigEndian(request.Data.Span[..4]);

        var thread = _database.BbsThreads
            .AsNoTracking()
            .Include(t => t.Posts)
            .ThenInclude(p => p.PostedBy)
            .FirstOrDefault(t => t.Id == threadId);

        if (thread == null)
        {
            return Task.FromResult<ICollection<FragmentMessage>>(new[]
            {
                new BbsThreadPostCountResponse(0).Build(),
            });
        }

        var responses = new List<FragmentMessage>
        {
            new BbsThreadPostCountResponse((uint)thread.Posts.Count).Build()
        };

        foreach (var post in thread.Posts)
        {
            responses.Add(new BbsThreadPostEntryResponse()
                .SetPostId((uint)post.Id)
                .SetPostedAt(post.CreatedAt)
                .SetPostTile(post.Title)
                .SetPostedByUsername(post.PostedBy.CharacterName)
                .Build());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
