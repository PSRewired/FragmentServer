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

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(OpCodes.Data, OpCodes.DataUpdateGuildShopItem)]
public class UpdateGuildShopItemRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public UpdateGuildShopItemRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);
        var guildId = reader.ReadUInt16();
        var itemId = reader.ReadInt32();
        var generalPrice = reader.ReadUInt32();
        var memberPrice = reader.ReadUInt32();
        var allowGeneral = reader.ReadBool();
        var allowMember = reader.ReadBool();

        _database.GuildShopItems
            .Where(gsi => gsi.Id == itemId && gsi.GuildId == guildId)
            .ExecuteUpdate(e =>
                e
                    .SetProperty(p => p.Price, v => generalPrice)
                    .SetProperty(p => p.MemberPrice, v => memberPrice)
                    .SetProperty(p => p.AvailableForGeneral, v => allowGeneral)
                    .SetProperty(p => p.AvailableForMember, v => allowMember)
                    .SetProperty(p => p.UpdatedAt, v => DateTime.UtcNow)
            );

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new UpdateGuildShopItemResponse().Build(),
        });
    }
}
