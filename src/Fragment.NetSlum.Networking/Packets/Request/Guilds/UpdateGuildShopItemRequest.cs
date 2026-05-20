using System;
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

[FragmentPacket(MessageType.Data, OpCodes.DataUpdateGuildShopItem)]
public class UpdateGuildShopItemRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ILogger<UpdateGuildShopItemRequest> _logger;

    public UpdateGuildShopItemRequest(FragmentContext database, ILogger<UpdateGuildShopItemRequest> logger)
    {
        _database = database;
        _logger = logger;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);
        var guildId = reader.ReadUInt16();
        var itemId = reader.ReadInt32();
        var generalPrice = reader.ReadUInt32();
        var memberPrice = reader.ReadUInt32();
        var allowGeneral = reader.ReadByte() == 0x01;
        var allowMember = reader.ReadByte() == 0x01;


        var gsi = _database.GuildShopItems.FirstOrDefault(gsi => gsi.ItemId == itemId && gsi.GuildId == guildId);

        if (gsi == null)
        {
            _logger.LogWarning("Player {PlayerId} ({PlayerName}) attempted to update unknown guild item {ItemId} for guild {GuildId}",
                session.CharacterId, session.CharacterInfo!.CharacterName, itemId, guildId);

            return SingleMessageAsync(new UpdateGuildShopItemResponse().Build());
        }

        gsi.Price = generalPrice;
        gsi.MemberPrice = memberPrice;
        gsi.AvailableForGeneral = allowGeneral;
        gsi.AvailableForMember = allowMember;

        _logger.LogWarning("Player {PlayerId} ({PlayerName}) updated availability/price to: General: {GeneralPrice}({AvailableGeneral}) Member: {MemberPrice}({AvailableMember}) for item {ItemId} in guild {GuildId}",
            session.CharacterId, session.CharacterInfo!.CharacterName, generalPrice, allowGeneral, memberPrice, allowMember, itemId, guildId);

        await _database.SaveChangesAsync();

        return SingleMessageAsync(new UpdateGuildShopItemResponse().Build());
    }
}
