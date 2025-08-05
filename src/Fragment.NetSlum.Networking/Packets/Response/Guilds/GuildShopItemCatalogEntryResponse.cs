using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildShopItemCatalogEntryResponse : BaseResponse
{
    private uint _shopId;
    private string _shopName = null!;
    private uint _price;
    private ushort _quantity;
    private uint _itemId;

    public GuildShopItemCatalogEntryResponse SetShopId(uint id)
    {
        _shopId = id;

        return this;
    }

    public GuildShopItemCatalogEntryResponse SetItemId(uint itemId)
    {
        _itemId = itemId;

        return this;
    }

    public GuildShopItemCatalogEntryResponse SetShopName(string shopName)
    {
        _shopName = shopName;

        return this;
    }

    public GuildShopItemCatalogEntryResponse SetAmount(uint price)
    {
        _price = price;

        return this;
    }

    public GuildShopItemCatalogEntryResponse SetQuantity(ushort quantity)
    {
        _quantity = quantity;

        return this;
    }


    public override FragmentMessage Build()
    {
        var nameBytes = _shopName.ToShiftJis();

        var writer = new MemoryWriter(nameBytes.Length + sizeof(uint) * 3 + sizeof(ushort));
        writer.Write(_shopId); // Guild ID
        writer.Write(nameBytes);
        writer.Write(_itemId); // Item ID
        writer.Write(_quantity); // Amount
        writer.Write(_price); // Price

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildShopItemCatalogEntryResponse,
            Data = writer.Buffer,
        };
    }
}
