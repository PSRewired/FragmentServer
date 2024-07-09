using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildShopItemList)]
public class GetGuildShopItemListRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetGuildShopItemListRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var guildId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        var guildItems = _database.GuildShopItems
            .Where(gsi => gsi.GuildId == guildId);

        var responses = new List<FragmentMessage>
        {
            new GuildItemListCountResponse((ushort)guildItems.Count()).Build(),
        };

        foreach (var item in guildItems)
        {
            responses.Add(new GuildItemListEntryResponse()
                .SetItemId((uint)item.Id)
                .SetQuantity(item.Quantity)
                .SetPrice(item.Price)
                .SetMemberPrice(item.MemberPrice)
                .SetGeneralAvailability(item.AvailableForGeneral)
                .SetMemberAvailability(item.AvailableForMember)
                .Build());
        }

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
