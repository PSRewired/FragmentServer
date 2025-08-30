using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Contexts;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Request;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildUpdateItemPricingAvailability)]
public class UpdateGuildItemPricingAvailabilityRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly GuildShopContextAccessor _guildShopContextAccessor;
    private readonly ILogger<UpdateGuildItemPricingAvailabilityRequest> _logger;

    public UpdateGuildItemPricingAvailabilityRequest(FragmentContext database, GuildShopContextAccessor guildShopContextAccessor,
        ILogger<UpdateGuildItemPricingAvailabilityRequest> logger)
    {
        _database = database;
        _guildShopContextAccessor = guildShopContextAccessor;
        _logger = logger;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var generalPrice = BinaryPrimitives.ReadUInt32BigEndian(request.Data.Span[..4]);
        var memberPrice = BinaryPrimitives.ReadUInt32BigEndian(request.Data.Span[4..8]);
        var isGeneral = Convert.ToBoolean(request.Data.Span[8]);
        var isMember = Convert.ToBoolean(request.Data.Span[9]);

        var guild = await _database.Guilds
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == session.GuildId);

        var isGuildMaster = guild != null && guild.LeaderId == session.GuildId;

        var donatedItem = _guildShopContextAccessor.Current.Donation;

        if (donatedItem == null)
        {
            _logger.LogWarning("Guild pricing availability was requested, but session does not have a donation in progress. Sending 0 quantity response");
            return SingleMessageAsync(new AddItemToGuildInventoryResponse(0).Build());
        }

        var guildItem = await _database.GuildShopItems
            .FirstOrDefaultAsync(i => i.ItemId == donatedItem.ItemId && i.GuildId == donatedItem.ToGuildId);

        if (guildItem == null)
        {
            _logger.LogWarning("While processing pricing availability request, guild item {GuildItemId} does not exist. Creating new entry", donatedItem.ItemId);
            guildItem = new GuildShopItem
            {
                ItemId = (int)donatedItem.ItemId,
                GuildId = session.GuildId,
                AvailableForGeneral = false,
                AvailableForMember = false,
            };
        }

        guildItem.Quantity += donatedItem.Quantity;

        if (isGuildMaster)
        {
            guildItem.Price = generalPrice;
            guildItem.MemberPrice = memberPrice;
            guildItem.AvailableForGeneral = isGeneral;
            guildItem.AvailableForMember = isMember;
        }

        _database.GuildShopItems.Update(guildItem);

        await _database.SaveChangesAsync();

        // Ensure we reset/remove the donation from the current context.
        _guildShopContextAccessor.Current.Donation = null;
        _logger.LogWarning("Player {PlayerId} ({PlayerName}) successfully donated {ItemQuantity} items with ID of {GuildItemId} to guild {GuildId}",
            session.CharacterId, session.CharacterInfo!.CharacterName, donatedItem.Quantity, donatedItem.ItemId, session.GuildId);

        return SingleMessageAsync(new AddItemToGuildInventoryResponse(donatedItem.Quantity).Build());
    }
}
