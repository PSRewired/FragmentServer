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

[FragmentPacket(MessageType.Data, OpCodes.DataGetGuildShopItems)]
public class GetGuildShopItemsRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetGuildShopItemsRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        //TODO: The GetShoppableGuildListRequest packet sets the shop ID to the guild's ID right now. This will need to be updated,
        //if we ever implement separate guild shops
        var shopId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        var isInGuild = _database.Characters.Any(c => c.Id == session.CharacterId && c.GuildId == shopId);

        var requestedGuildItems = _database.GuildShopItems
            .Where(gs => gs.GuildId == shopId)
            .Select(gs => new
            {
                ItemId = gs.ItemId,
                Quantity = gs.Quantity,
                Price = isInGuild ? gs.MemberPrice : gs.Price
            });

        var responses = new List<FragmentMessage>
        {
            new GuildShopItemCountResponse((ushort)requestedGuildItems.Count()).Build()
        };

        foreach (var item in requestedGuildItems)
        {
            responses.Add(new GuildShopItemEntryResponse()
                .SetItemId((uint)item.ItemId)
                .SetQuantity(item.Quantity)
                .SetPrice(item.Price)
                .Build());
        }

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
