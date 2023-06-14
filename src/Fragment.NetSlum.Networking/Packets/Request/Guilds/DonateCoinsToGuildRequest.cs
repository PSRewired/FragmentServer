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

[FragmentPacket(OpCodes.Data, OpCodes.DataDonateCoinsToGuild)]
public class DonateCoinsToGuildRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public DonateCoinsToGuildRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);

        var guildId = reader.ReadUInt16();
        var goldDonated = reader.ReadUInt16();
        var silverDonated = reader.ReadUInt16();
        var bronzeDonated = reader.ReadUInt16();

        var guildStats = _database.GuildStats
            .First(g => g.Id == guildId);

        guildStats.GoldAmount += goldDonated;
        guildStats.SilverAmount += silverDonated;
        guildStats.BronzeAmount += bronzeDonated;

        _database.SaveChanges();

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new DonateCoinsToGuildResponse(goldDonated, silverDonated, bronzeDonated).Build(),
        });
    }
}
