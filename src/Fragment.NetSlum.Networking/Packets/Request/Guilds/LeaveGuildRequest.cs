using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
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

[FragmentPacket(MessageType.Data, OpCodes.DataLeaveGuild)]
public class LeaveGuildRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public LeaveGuildRequest(FragmentContext database)
    {
        _database = database;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var myCharacter = _database.Characters
            .Include(c => c.Guild)
            .First(c => c.Id == session.CharacterId);

        if (myCharacter.Guild == null)
        {
            throw new DataException("Player {PlayerName} requested to leave a guild, but is not currently in one");
        }

        myCharacter.GuildId = null;

        _database.Add(new GuildActivityLog
        {
            ActionPerformed = GuildActivityLog.GuildPlayerAction.PlayerLeft,
            GuildId = myCharacter.Guild.Id,
            PerformedByCharacterId = session.CharacterId,
            PerformedOnCharacterId = session.CharacterId,
        });

        await _database.SaveChangesAsync();

        return ReturnSingleAsync(new LeaveGuildResponse().Build());
    }
}
