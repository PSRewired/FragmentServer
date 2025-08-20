using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Commands.News;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Articles;
using Fragment.NetSlum.Networking.Queries.Images;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Articles;

[FragmentPacket(MessageType.Data, OpCodes.DataNewsGetPost)]
public class GetNewsPostRequest : BaseRequest
{
    private readonly ICommandBus _commandBus;
    private readonly FragmentContext _database;

    public GetNewsPostRequest(ICommandBus commandBus, FragmentContext database)
    {
        _commandBus = commandBus;
        _database = database;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var articleId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        await _commandBus.Execute<bool>(new MarkNewsArticleReadCommand(articleId, session.PlayerAccountId));

        var post = _database.WebNewsArticles
            .AsNoTracking()
            .FirstOrDefault(a => a.Id == articleId);

        if (post?.Image == null)
        {
            //return new[] { new GetNewsPostErrorResponse().Build() };
            return new[]
            {
                new NewsPostImageSizeResponse((uint)0, 0).Build(),
                new NewsPostImageDetailsResponse(Array.Empty<byte>()).Build()
            };
        }

        var articleImageInfo = await _commandBus.GetResult(new GetImageInfoQuery(post.Image));

        return new[]
        {
            //new NewsPostImageSizeResponse((uint)0, 0).Build(),
            new NewsPostImageSizeResponse((uint)articleImageInfo.ImageData.Length, articleImageInfo.ChunkCount).Build(),
            new NewsPostImageDetailsResponse(articleImageInfo.ImageData).Build()
        };
    }
}
