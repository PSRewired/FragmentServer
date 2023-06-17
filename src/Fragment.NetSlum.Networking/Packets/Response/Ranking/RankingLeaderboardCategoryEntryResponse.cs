using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Ranking;

public class RankingLeaderboardCategoryEntryResponse : BaseResponse
{
    private ushort _categoryId;
    private string _categoryName = "";

    public RankingLeaderboardCategoryEntryResponse SetCategoryId(ushort id)
    {
        _categoryId = id;

        return this;
    }

    public RankingLeaderboardCategoryEntryResponse SetCategoryName(string name)
    {
        _categoryName = name;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _categoryName.ToShiftJis();

        var writer = new MemoryWriter(nameBytes.Length + sizeof(ushort));
        writer.Write(_categoryId);
        writer.Write(nameBytes);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.RankingLeaderboardCategoryEntryResponse,
            Data = writer.Buffer,
        };
    }
}
