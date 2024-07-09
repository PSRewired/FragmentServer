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
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataPurchaseGuildShopItem)]
public class PurchaseGuildShopItemRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public PurchaseGuildShopItemRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);

        var guildId = reader.ReadUInt16();
        var itemId = reader.ReadUInt32();
        var quantityPurchased = reader.ReadUInt16();
        var unitPrice = reader.ReadUInt32();

        var guildShopItem = _database.GuildShopItems
            .Include(gs => gs.Guild)
            .ThenInclude(g => g.Stats)
            .First(gs => gs.GuildId == guildId && gs.Id == itemId);

        if (guildShopItem.Quantity - quantityPurchased < 0)
        {
            throw new DataException(
                $"Player attempted to purchase {quantityPurchased} of itemID {itemId} but only {guildShopItem.Quantity} are available");
        }

        guildShopItem.Quantity -= quantityPurchased;
        guildShopItem.Guild.Stats.CurrentGp += (int)unitPrice * quantityPurchased;

        _database.SaveChanges();

        return SingleMessage(new PurchaseGuildShopItemResponse(quantityPurchased).Build());
    }
}
