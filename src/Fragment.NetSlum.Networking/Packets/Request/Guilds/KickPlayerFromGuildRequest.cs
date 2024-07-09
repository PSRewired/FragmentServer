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
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildKickPlayer)]
public class KickPlayerFromGuildRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public KickPlayerFromGuildRequest(FragmentContext database)
    {
        _database = database;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var kickedPlayerId = BinaryPrimitives.ReadInt32BigEndian(request.Data.Span[..4]);

        await _database.Characters
            .Where(c => c.Id == kickedPlayerId)
            .ExecuteUpdateAsync(c => c.SetProperty(p => p.GuildId, p => null));

        var guild = _database.Guilds
            .AsNoTracking()
            .First(g => g.LeaderId == session.CharacterId);

        _database.Add(new GuildActivityLog
        {
            ActionPerformed = GuildActivityLog.GuildPlayerAction.PlayerKicked,
            GuildId = guild.Id,
            PerformedByCharacterId = session.CharacterId,
            PerformedOnCharacterId = kickedPlayerId,
        });

        await _database.SaveChangesAsync();

        return new[] { new KickPlayerFromGuildResponse().Build() };
    }
}
