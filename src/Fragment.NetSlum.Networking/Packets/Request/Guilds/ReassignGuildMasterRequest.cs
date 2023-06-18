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

[FragmentPacket(MessageType.Data, OpCodes.ReassignGuildMaster)]
public class ReassignGuildMasterRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public ReassignGuildMasterRequest(FragmentContext database)
    {
        _database = database;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var newMasterId = BinaryPrimitives.ReadInt32BigEndian(request.Data.Span[..4]);

        var myCharacter = _database.Characters.First(c => c.Id == session.CharacterId);

        // When master is re-assigned, the original leader has left the guild
        myCharacter.GuildId = null;

        var myGuild = _database.Guilds
            .First(g => g.LeaderId == session.CharacterId);

        myGuild.LeaderId = newMasterId;

        await _database.SaveChangesAsync();

        return new[] { new ReassignGuildMasterResponse().Build() };
    }
}
