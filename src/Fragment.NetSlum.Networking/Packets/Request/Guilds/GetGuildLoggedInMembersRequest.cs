using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildLoggedInMembers)]
public class GetGuildLoggedInMembersRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetGuildLoggedInMembersRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort guildId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        var guild = _database.Guilds
            .AsNoTracking()
            .Include(g => g.Members)
            .Include(g => g.Leader)
            .Include(g => g.Stats)
            .First(g => g.Id == guildId);

        var guildShopInfoQuery = _database.GuildShopItems
            .AsNoTracking()
            .Where(gs => gs.GuildId == guildId);

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new GuildLoggedInMembersResponse()
                .SetGuildName(guild.Name)
                .SetGuildDescription(guild.Comment)
                .SetGuildEmblem(guild.Emblem)
                .SetCreatedAtDate(guild.CreatedAt)
                .SetGoldCount((uint)guild.Stats.GoldAmount)
                .SetSilverCount((uint)guild.Stats.SilverAmount)
                .SetBronzeCount((uint)guild.Stats.BronzeAmount)
                .SetLeaderName(guild.Leader?.CharacterName ?? "Unknown")
                .SetTotalGp((uint)guild.Stats.CurrentGp)
                .SetMemberCount((ushort)guild.Members.Count)
                .SetLongArmCount((ushort)guild.Members.Count(m => m.Class == CharacterClass.LongArm))
                .SetTwinBladeCount((ushort)guild.Members.Count(m => m.Class == CharacterClass.TwinBlade))
                .SetHeavyAxeCount((ushort)guild.Members.Count(m => m.Class == CharacterClass.AxeHeavy))
                .SetBladeMasterCount((ushort)guild.Members.Count(m => m.Class == CharacterClass.BladeMaster))
                .SetWaveMasterCount((ushort)guild.Members.Count(m => m.Class == CharacterClass.WaveMaster))
                .SetHeavyBladeCount((ushort)guild.Members.Count(m => m.Class == CharacterClass.HeavyBlade))
                .SetAverageLevel((ushort)(guild.Members.Sum(m => m.CurrentLevel) / guild.Members.Count))
                .SetGeneralItemsAvailable((uint)guildShopInfoQuery.Count(gs => gs.AvailableForGeneral))
                .SetMemberItemsAvailable((uint)guildShopInfoQuery.Count(gs => gs.AvailableForMember))
                .SetNumberJoined(0) //TODO: I have no idea what this is actually supposed to represent in the 'guild scale' section of the stats..
                .Build()
        });
    }
}
