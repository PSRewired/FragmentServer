using System.Buffers.Binary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Contexts;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildDonateItem)]
public class DonateItemToGuildRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly GuildShopContextAccessor _guildShopContextAccessor;
    private readonly ILogger<DonateItemToGuildRequest> _logger;

    public DonateItemToGuildRequest(FragmentContext database, GuildShopContextAccessor guildShopContextAccessor, ILogger<DonateItemToGuildRequest> logger)
    {
        _database = database;
        _guildShopContextAccessor = guildShopContextAccessor;
        _logger = logger;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var guildId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);
        var guildItemId = BinaryPrimitives.ReadUInt32BigEndian(request.Data.Span[2..6]);
        var itemQuantity = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[6..8]);

        var guildItem = await _database.GuildShopItems
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.ItemId == guildItemId && i.GuildId == guildId);

        if (guildItem == null)
        {
            _logger.LogWarning("Player {PlayerId}({PlayerName}) attempted to donate unknown item {ItemId} to guild {GuildId}", session.CharacterId, session.CharacterInfo!.CharacterName, guildItemId, session.GuildId);
            return SingleMessageAsync(new GuildItemPriceResponse()
                .Build());
        }

        _guildShopContextAccessor.Current.Donation = new GuildShopContextAccessor.GuildShopItemDonation(guildId, guildItemId, itemQuantity);

        _logger.LogInformation("Player {PlayerId}({PlayerName}) is donating item {ItemId} to guild {GuildId}", session.CharacterId, session.CharacterInfo!.CharacterName, guildItemId, session.GuildId);

        return SingleMessageAsync(new GuildItemPriceResponse()
            .SetGeneralPrice(guildItem.Price)
            .SetMemberPrice(guildItem.MemberPrice)
            .Build());
    }
}
