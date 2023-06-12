using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(OpCodes.Data, OpCodes.DataGuildMemberListRequest)]
public class GuildMemberListRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GuildMemberListRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort categoryId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        if (categoryId == 0)
        {
            return Task.FromResult(GetMemberListCategories());
        }

        var myPlayer = _database.Characters.First(c => c.Id == session.CharacterId);

        var memberQuery = _database.Characters
            .Include(c => c.Guild)
            .Where(c => c.GuildId == myPlayer.GuildId);

        if (categoryId != ushort.MaxValue)
        {
            memberQuery = memberQuery
                .Where(m => m.Class == (CharacterClass)categoryId && m.Id != myPlayer.Id);
        }

        var responses = new List<FragmentMessage>
        {
            new GuildMemberListEntryCountResponse((ushort)memberQuery.Count()).Build()
        };

        foreach (var member in memberQuery)
        {
            responses.Add(new GuildMemberListEntryResponse()
                .SetMemberId((uint)member.Id)
                .SetClass(member.Class)
                .SetModelId(member.FullModelId)
                .SetLevel((ushort)member.CurrentLevel)
                .SetMemberGreeting(member.GreetingMessage)
                .SetMemberName(member.CharacterName)
                .SetMembershipStatus(member.Guild!.LeaderId == myPlayer.Id ? GuildStatus.GuildMaster : GuildStatus.Member)
                .SetIsOnline(session.Server.Sessions
                    .Cast<FragmentTcpSession>()
                    .Any(s => s.CharacterId == member.Id))
                .Build());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }

    private ICollection<FragmentMessage> GetMemberListCategories()
    {
        var classTypes = Enum.GetValues(typeof(CharacterClass));

        var responses = new List<FragmentMessage>
        {
            new GuildMemberListCategoryCountResponse((ushort)(classTypes.Length + 1)).Build(),
            new GuildMemberListCategoryEntryResponse()
                .SetCategoryId(ushort.MaxValue)
                .SetCategoryName("ALL")
                .Build()
        };

        foreach (CharacterClass classType in classTypes)
        {
            responses.Add(new GuildMemberListCategoryEntryResponse()
                .SetCategoryId((ushort) classType)
                .SetCategoryName(classType.GetClassName())
                .Build()
            );
        }

        return responses;
    }
}
