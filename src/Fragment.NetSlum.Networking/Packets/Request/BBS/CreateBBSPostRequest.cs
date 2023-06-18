using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.BBS;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Networking.Packets.Request.BBS;

[FragmentPacket(MessageType.Data, OpCodes.DataBbsPost)]
public class CreateBBSPostRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public CreateBBSPostRequest(FragmentContext database)
    {
        _database = database;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var threadId = BinaryPrimitives.ReadUInt32BigEndian(request.Data.Span[..4]);
        var username = request.Data.Span[4..20].ToShiftJisString();
        var postTitle = request.Data.Span[84..116].ToShiftJisString();
        var postBody = request.Data.Span[134..734].ToShiftJisString();

        var threadForPost = _database.BbsThreads.FirstOrDefault(t => t.Id == threadId);
        var postCreationCharacter = _database.Characters.First(c => c.CharacterName == username);

        if (threadForPost == null)
        {
            threadForPost = new BbsThread
            {
                Title = postTitle,
                CategoryId = 1, //TODO: Work out categories
            };

            _database.BbsThreads.Add(threadForPost);
        }

        var post = new BbsPost
        {
            Title = postTitle,
            PostedBy = postCreationCharacter,
            Thread = threadForPost,
        };

        var postContent = new BbsPostContent
        {
            Content = postBody,
            Post = post,
        };

        _database.BbsPosts.Add(post);
        _database.BbsPostContents.Add(postContent);

        await _database.SaveChangesAsync();

        return new[]
        {
            new CreateBBSPostResponse().Build(),
        };
    }
}
