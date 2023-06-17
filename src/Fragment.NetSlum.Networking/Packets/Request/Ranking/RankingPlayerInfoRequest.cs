using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Ranking;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Ranking;

[FragmentPacket(OpCodes.Data, OpCodes.RankPlayerInfo)]
public class RankingPlayerInfoRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public RankingPlayerInfoRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var characterId = BinaryPrimitives.ReadInt32BigEndian(request.Data.Span[..4]);

        var character = _database.Characters
            .Include(c => c.Guild)
            .First(c => c.Id == characterId);

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new RankingPlayerInfoResponse()
                .SetClass(character.Class)
                .SetModelId(character.FullModelId)
                .SetLevel((ushort)character.CurrentLevel)
                .SetMemberGreeting(character.GreetingMessage)
                .SetMemberName(character.CharacterName)
                .SetMembershipStatus(character.Guild == null ? GuildStatus.None : character.Guild.LeaderId == character.Id ? GuildStatus.GuildMaster : GuildStatus.Member)
                .SetIsOnline(session.Server.Sessions
                    .Cast<FragmentTcpSession>()
                    .Any(s => s.CharacterId == characterId))
                .Build(),
        });
    }
}
