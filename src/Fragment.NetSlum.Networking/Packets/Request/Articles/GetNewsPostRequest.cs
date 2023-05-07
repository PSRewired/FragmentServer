using System.Buffers.Binary;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Commands.News;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Articles;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Articles;

[FragmentPacket(OpCodes.Data, OpCodes.DataNewsGetPost)]
public class GetNewsPostRequest : BaseRequest
{
    private readonly ICommandBus _commandBus;

    public GetNewsPostRequest(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var articleId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        await _commandBus.Execute<bool>(new MarkNewsArticleReadCommand(articleId, session.PlayerAccountId));

        return new[] { new GetNewsPostErrorResponse().Build() };
    }
}
