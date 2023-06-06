
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildLoggedInMembersResponse : BaseResponse
{
    private string _guildName = "";
    private string _guildDescription = "";
    private string _leaderName = "";

    private Memory<byte> _guildEmblem = Array.Empty<byte>();
    private DateTime _createdAt = DateTime.MinValue;

    private ushort _memberCount;
    private ushort _numTwinBlades;
    private ushort _numBladeMasters;
    private ushort _numHeavyBlades;
    private ushort _numHeavyAxes;
    private ushort _numLongArms;
    private ushort _numWaveMasters;
    private ushort _averageLevel;

    private uint _bronzeCount;
    private uint _silverCount;
    private uint _goldCount;
    private uint _gpCount;

    public GuildLoggedInMembersResponse SetGuildName(string name)
    {
        _guildName = name;

        return this;
    }

    public GuildLoggedInMembersResponse SetGuildDescription(string name)
    {
        _guildDescription = name;

        return this;
    }

    public GuildLoggedInMembersResponse SetGuildEmblem(Memory<byte> emblem)
    {
        _guildEmblem = emblem;

        return this;
    }

    public GuildLoggedInMembersResponse SetCreatedAtDate(DateTime date)
    {
        _createdAt = date;

        return this;
    }

    public GuildLoggedInMembersResponse SetLeaderName(string name)
    {
        _leaderName = name;

        return this;
    }

    public GuildLoggedInMembersResponse SetMemberCount(ushort count)
    {
        _memberCount = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetTwinBladeCount(ushort count)
    {
        _numTwinBlades = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetBladeMasterCount(ushort count)
    {
        _numBladeMasters = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetHeavyBladeCount(ushort count)
    {
        _numHeavyBlades = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetHeavyAxeCount(ushort count)
    {
        _numHeavyAxes = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetLongArmCount(ushort count)
    {
        _numLongArms = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetWaveMasterCount(ushort count)
    {
        _numWaveMasters = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetAverageLevel(ushort level)
    {
        _averageLevel = level;

        return this;
    }

    public GuildLoggedInMembersResponse SetBronzeCount(uint count)
    {
        _bronzeCount = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetSilverCount(uint count)
    {
        _silverCount = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetGoldCount(uint count)
    {
        _goldCount = count;

        return this;
    }

    public GuildLoggedInMembersResponse SetTotalGp(uint count)
    {
        _gpCount = count;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _guildName.ToShiftJis();
        var descriptionBytes = _guildDescription.ToShiftJis();
        var timestampBytes = _createdAt.ToString("yyyy/MM/dd").ToShiftJis();
        var leaderNameBytes = _leaderName.ToShiftJis();

        var writer = new MemoryWriter(sizeof(ushort) * 8 + sizeof(uint) * 4 + _guildEmblem.Length + nameBytes.Length +
                                      descriptionBytes.Length + timestampBytes.Length + leaderNameBytes.Length + 2);

        writer.Write(nameBytes);
        writer.Write(timestampBytes);
        writer.Write(leaderNameBytes);
        writer.Write(_memberCount);
        writer.Write(_numTwinBlades);
        writer.Write(_numBladeMasters);
        writer.Write(_numHeavyBlades);
        writer.Write(_numHeavyAxes);
        writer.Write(_numLongArms);
        writer.Write(_numWaveMasters);
        writer.Write(_averageLevel);
        writer.Write(_goldCount);
        writer.Write(_silverCount);
        writer.Write(_bronzeCount);
        writer.Write(_gpCount);
        writer.Write(descriptionBytes);
        writer.Write(_guildEmblem);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataGuildLoggedInMembersResponse,
            Data = writer.Buffer,
        };
    }
}
