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

[FragmentPacket(MessageType.Data, OpCodes.DataDissolveGuild)]
public class DissolveGuildRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public DissolveGuildRequest(FragmentContext database)
    {
        _database = database;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        await _database.Guilds
            .Where(g => g.LeaderId == session.CharacterId)
            .ExecuteDeleteAsync();

        return new[] { new DissolveGuildResponse().Build() };
    }
}
