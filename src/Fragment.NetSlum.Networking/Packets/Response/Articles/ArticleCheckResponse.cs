using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Articles;

public class ArticleCheckResponse : BaseResponse
{
    private bool _articlesAvailable;

    public ArticleCheckResponse ArticlesAvailable(bool available)
    {
        _articlesAvailable = available;

        return this;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, (ushort)(_articlesAvailable ? 0x01 : 0x00));

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataNewCheckOk,
            Data = buffer,
        };
    }
}
