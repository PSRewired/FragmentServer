using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildShopItemCountResponse : BaseResponse
{
    private readonly ushort _numItems;

    public GuildShopItemCountResponse(ushort numItems)
    {
        _numItems = numItems;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _numItems);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataGetShopItemCountResponse,
            Data = buffer,
        };
    }
}
