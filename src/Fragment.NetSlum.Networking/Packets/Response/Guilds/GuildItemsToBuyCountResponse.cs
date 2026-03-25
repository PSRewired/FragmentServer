using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildItemsToBuyCountResponse : BaseResponse
{
    private readonly ushort _numItems;

    public GuildItemsToBuyCountResponse(ushort numItems)
    {
        _numItems = numItems;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _numItems);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildGetItemsToBuyCountResponse,
            Data = buffer,
        };
    }
}
