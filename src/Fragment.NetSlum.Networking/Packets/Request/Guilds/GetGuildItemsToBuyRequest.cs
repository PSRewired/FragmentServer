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
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildGetItemsToBuy)]
public class GetGuildItemsToBuyRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetGuildItemsToBuyRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var guildId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        var guildItems = _database.GuildShopItems
            .AsNoTracking()
            .Where(g => g.GuildId == guildId && g.AvailableForMember);

        var responses = new List<FragmentMessage>
        {
            new GuildItemsToBuyCountResponse((ushort)guildItems.Count()).Build(),
        };

        foreach (var item in guildItems)
        {
            responses.Add(new GuildItemToBuyResponse()
                .SetItemId((uint)item.ItemId)
                .SetQuantity(item.Quantity)
                .SetPrice(item.MemberPrice)
                .Build());
        }

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
