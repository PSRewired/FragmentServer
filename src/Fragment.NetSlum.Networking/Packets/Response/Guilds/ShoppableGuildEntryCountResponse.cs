using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class ShoppableGuildEntryCountResponse : BaseResponse
{
    private readonly ushort _numCategories;

    public ShoppableGuildEntryCountResponse(ushort numCategories)
    {
        _numCategories = numCategories;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _numCategories);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.ShoppableGuildEntryCountResponse,
            Data = buffer,
        };
    }
}
