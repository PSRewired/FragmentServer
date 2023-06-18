using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Ranking;

public class RankingLeaderboardPlayerEntryResponse : BaseResponse
{
    private uint _playerId;
    private string _playerName = "";

    public RankingLeaderboardPlayerEntryResponse SetPlayerId(uint id)
    {
        _playerId = id;

        return this;
    }

    public RankingLeaderboardPlayerEntryResponse SetPlayerName(string name)
    {
        _playerName = name;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _playerName.ToShiftJis();

        var writer = new MemoryWriter(nameBytes.Length + sizeof(uint));
        writer.Write(nameBytes);
        writer.Write(_playerId);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.RankingLeaderboardPlayerEntryResponse,
            Data = writer.Buffer,
        };
    }
}
