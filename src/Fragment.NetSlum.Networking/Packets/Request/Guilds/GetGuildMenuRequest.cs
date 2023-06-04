using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(OpCodes.Data, OpCodes.DataGuildGetMenu)]
public class GetGuildMenuRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetGuildMenuRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort menuId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        if (menuId < 1)
        {
            return Task.FromResult(HandleMenuCategories());
        }

        var responses = new List<FragmentMessage>();

        var guilds = _database.Guilds
            .AsNoTracking();

        switch (menuId)
        {
            // Top 10
            case 2:
                guilds = guilds
                    .Include(g => g.Stats)
                    .OrderByDescending(g => g.Stats.CurrentGp)
                    .Take(10);
                break;
            // Most Members
            case 3:
                guilds = guilds
                    .Include(g => g.Members)
                    .OrderByDescending(g => g.Members.Count);
                break;
        }

        responses.Add(new GuildListEntryCountResponse((ushort)guilds.Count()).Build());

        foreach (var guild in guilds)
        {
            responses.Add(new GuildMenuListEntryResponse()
                .SetGuildId(guild.Id)
                .SetGuildName(menuId == 3 ? $"({guild.Members.Count}){guild.Name}" : guild.Name)
                .Build()
            );
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }

    private static ICollection<FragmentMessage> HandleMenuCategories()
    {
        return new[]
        {
            new GuildMenuCategoryCountResponse(3).Build(),
            new GuildMenuCategoryResponse()
                .SetCategoryId(1)
                .SetCategoryName("ALL")
                .Build(),
            new GuildMenuCategoryResponse()
                .SetCategoryId(2)
                .SetCategoryName("TOP 10")
                .Build(),

            new GuildMenuCategoryResponse()
                .SetCategoryId(3)
                .SetCategoryName("MOST MEMBERS")
                .Build(),
        };
    }
}
