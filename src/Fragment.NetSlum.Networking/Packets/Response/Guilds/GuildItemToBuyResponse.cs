using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildItemToBuyResponse : BaseResponse
{
    private uint _itemId;
    private ushort _quantity;
    private uint _price;

    public GuildItemToBuyResponse SetItemId(uint itemId)
    {
        _itemId = itemId;

        return this;
    }

    public GuildItemToBuyResponse SetQuantity(ushort quantity)
    {
        _quantity = quantity;

        return this;
    }

    public GuildItemToBuyResponse SetPrice(uint price)
    {
        _price = price;

        return this;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(uint) * 2 + sizeof(ushort));

        writer.Write(_itemId);
        writer.Write(_quantity);
        writer.Write(_price);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildGetItemToBuyResponse,
            Data = writer.Buffer,
        };
    }
}
