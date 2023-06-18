using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildItemListEntryResponse : BaseResponse
{
    private uint _itemId;
    private ushort _quantity;
    private uint _price;
    private uint _memberPrice;
    private bool _availableForMember;
    private bool _availableForGeneral;

    public GuildItemListEntryResponse SetItemId(uint id)
    {
        _itemId = id;

        return this;
    }

    public GuildItemListEntryResponse SetQuantity(ushort quantity)
    {
        _quantity = quantity;

        return this;
    }

    public GuildItemListEntryResponse SetPrice(uint price)
    {
        _price = price;

        return this;
    }

    public GuildItemListEntryResponse SetMemberPrice(uint price)
    {
        _memberPrice = price;

        return this;
    }

    public GuildItemListEntryResponse SetMemberAvailability(bool available)
    {
        _availableForMember = available;

        return this;
    }

    public GuildItemListEntryResponse SetGeneralAvailability(bool available)
    {
        _availableForGeneral = available;

        return this;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(uint) * 3 +
                                      sizeof(ushort) +
                                      2);

        writer.Write(_itemId);
        writer.Write(_quantity);
        writer.Write(_price);
        writer.Write(_memberPrice);
        writer.Write(_availableForGeneral);
        writer.Write(_availableForMember);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildItemListEntryResponse,
            Data = writer.Buffer,
        };
    }
}
