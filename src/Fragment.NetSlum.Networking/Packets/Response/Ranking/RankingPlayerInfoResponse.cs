using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Ranking;

public class RankingPlayerInfoResponse : BaseResponse
{
    private ushort _characterLevel;

    private uint _characterModelId;

    private string _memberName = null!;
    private string _memberGreeting = "";

    private string _guildName = "";
    private GuildStatus _guildStatus;
    private CharacterClass _memberClass;

    private bool _isOnline;

    public RankingPlayerInfoResponse SetMemberName(string name)
    {
        _memberName = name;

        return this;
    }

    public RankingPlayerInfoResponse SetMemberGreeting(string greeting)
    {
        _memberGreeting = greeting;

        return this;
    }

    public RankingPlayerInfoResponse SetClass(CharacterClass cls)
    {
        _memberClass = cls;

        return this;
    }

    public RankingPlayerInfoResponse SetModelId(uint modelId)
    {
        _characterModelId = modelId;

        return this;
    }

    public RankingPlayerInfoResponse SetLevel(ushort level)
    {
        _characterLevel = level;

        return this;
    }

    public RankingPlayerInfoResponse SetIsOnline(bool online)
    {
        _isOnline = online;

        return this;
    }

    public RankingPlayerInfoResponse SetMembershipStatus(GuildStatus status)
    {
        _guildStatus = status;

        return this;
    }

    public RankingPlayerInfoResponse SetGuildName(string name)
    {
        _guildName = name;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _memberName.ToShiftJis();
        var greetingBytes = _memberGreeting.ToShiftJis();
        var guildNameBytes = _guildName.ToShiftJis();

        var writer = new MemoryWriter(
            nameBytes.Length +
            greetingBytes.Length +
            guildNameBytes.Length +
            sizeof(ushort) * 2 +
            sizeof(uint) * 2
            + 1);

        writer.Write(nameBytes);
        writer.Write((byte)_memberClass);
        writer.Write(_characterLevel);
        writer.Write(greetingBytes);
        writer.Write(guildNameBytes);
        writer.Write(_characterModelId);
        writer.Write((byte)(_isOnline ? 1 : 0));
        writer.Write((byte)_guildStatus);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.RankPlayerInfoResponse,
            Data = writer.Buffer,
        };
    }
}
