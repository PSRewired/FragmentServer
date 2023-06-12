using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(OpCodes.Data, OpCodes.DataGuildGetAllGuilds)]
public class GetShoppableGuildListRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetShoppableGuildListRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort guildId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        //This intentionally works differently than the older servers. The guild list is returned first.
        //TODO: Create different shop types?
        if (guildId < 1)
        {
            return Task.FromResult(HandleGuildList());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new GuildShopEntryCountResponse(1).Build(),
            new GuildShopEntryResponse()
                .SetGuildId(guildId)
                .SetGuildName("ALL")
                .Build(),
        });
    }

    private ICollection<FragmentMessage> HandleGuildList()
    {
        var responses = new List<FragmentMessage>();

        var guildShops = _database.GuildShopItems
            .AsNoTracking()
            .Include(gs => gs.Guild)
            .GroupBy(gs => gs.GuildId)
            .Select(g => g.First());


        responses.Add(new ShoppableGuildEntryCountResponse((ushort)guildShops.Count()).Build());

        foreach (var shop in guildShops)
        {
            responses.Add(new ShoppableGuildEntryResponse()
                .SetCategoryId(shop.Guild.Id)
                .SetCategoryName(shop.Guild.Name)
                .Build()
            );
        }

        return responses;
    }
}
