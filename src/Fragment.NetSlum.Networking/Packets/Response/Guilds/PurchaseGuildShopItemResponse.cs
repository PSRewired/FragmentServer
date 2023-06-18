using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class PurchaseGuildShopItemResponse : BaseResponse
{
    private readonly ushort _quantityPurchased;

    public PurchaseGuildShopItemResponse(ushort quantityPurchased)
    {
        _quantityPurchased = quantityPurchased;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _quantityPurchased);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataPurchaseGuildShopItemResponse,
            Data = buffer,
        };
    }
}
