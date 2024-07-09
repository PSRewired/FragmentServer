using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataTakeGuildShopItem)]
public class TakeGuildShopItemRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public TakeGuildShopItemRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);
        var guildId = reader.ReadUInt16();
        var itemId = reader.ReadUInt32();
        var quantity = reader.ReadUInt16();

        var guildShopItem = _database.GuildShopItems.First(gsi => gsi.ItemId == itemId && gsi.GuildId == guildId);

        if (guildShopItem.Quantity - quantity < 0)
        {
            throw new DataException(
                $"Player attempted to purchase {quantity} of itemID {itemId} but only {guildShopItem.Quantity} are available");
        }

        guildShopItem.Quantity -= quantity;
        _database.SaveChanges();

        return SingleMessage(new TakeGuildShopItemResponse(quantity).Build());
    }
}
