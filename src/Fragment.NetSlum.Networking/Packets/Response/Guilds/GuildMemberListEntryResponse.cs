using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildMemberListEntryResponse : BaseResponse
{
    private ushort _characterLevel;

    private uint _characterModelId;
    private uint _memberId;

    private string _memberName = null!;
    private string _memberGreeting = "";

    private GuildStatus _guildStatus;
    private CharacterClass _memberClass = default!;

    private bool _isOnline;

    public GuildMemberListEntryResponse SetMemberId(uint id)
    {
        _memberId = id;

        return this;
    }

    public GuildMemberListEntryResponse SetMemberName(string name)
    {
        _memberName = name;

        return this;
    }

    public GuildMemberListEntryResponse SetMemberGreeting(string greeting)
    {
        _memberGreeting = greeting;

        return this;
    }

    public GuildMemberListEntryResponse SetClass(CharacterClass cls)
    {
        _memberClass = cls;

        return this;
    }

    public GuildMemberListEntryResponse SetModelId(uint modelId)
    {
        _characterModelId = modelId;

        return this;
    }

    public GuildMemberListEntryResponse SetLevel(ushort level)
    {
        _characterLevel = level;

        return this;
    }

    public GuildMemberListEntryResponse SetIsOnline(bool online)
    {
        _isOnline = online;

        return this;
    }

    public GuildMemberListEntryResponse SetMembershipStatus(GuildStatus status)
    {
        _guildStatus = status;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _memberName.ToShiftJis();
        var greetingBytes = _memberGreeting.ToShiftJis();

        var writer = new MemoryWriter(
            nameBytes.Length +
            greetingBytes.Length +
            sizeof(ushort) * 2 +
            sizeof(uint) * 2
            + 1);

        writer.Write(nameBytes);
        writer.Write((byte)_memberClass);
        writer.Write(_characterLevel);
        writer.Write(greetingBytes);
        writer.Write((byte)(_isOnline ? 1 : 0));
        writer.Write(_characterModelId);
        writer.Write(_memberId);
        writer.Write((byte)_guildStatus);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataGuildMemberListMemberEntryResponse,
            Data = writer.Buffer,
        };
    }
}
