using System.Collections.Generic;
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
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataPurchaseGuildShopItem)]
public class PurchaseGuildShopItemRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ILogger<PurchaseGuildShopItemRequest> _logger;

    public PurchaseGuildShopItemRequest(FragmentContext database, ILogger<PurchaseGuildShopItemRequest> logger)
    {
        _database = database;
        _logger = logger;
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
            .FirstOrDefault(gs => gs.GuildId == guildId && gs.ItemId == itemId);


        if (guildShopItem == null)
        {
            _logger.LogWarning("Player attempted to buy unknown item {ItemId} from guild {GuildId} for {UnitPrice}x{Quantity}", itemId, guildId, unitPrice, quantityPurchased);
            return SingleMessage(new PurchaseGuildShopItemResponse(0).Build());
        }

        _logger.LogInformation("Player is buying item {ItemId} from guild {GuildId} for {UnitPrice}x{Quantity}", itemId, guildId, unitPrice, quantityPurchased);

        if (guildShopItem.Quantity - quantityPurchased < 0)
        {
            return SingleMessage(new PurchaseGuildShopItemResponse(guildShopItem.Quantity).Build());
            // throw new DataException(
            //     $"Player attempted to purchase {quantityPurchased} of itemID {itemId} but only {guildShopItem.Quantity} are available");
        }

        guildShopItem.Quantity -= quantityPurchased;
        guildShopItem.Guild.Stats.CurrentGp += (int)unitPrice * quantityPurchased;

        _database.SaveChanges();

        return SingleMessage(new PurchaseGuildShopItemResponse(quantityPurchased).Build());
    }
}
