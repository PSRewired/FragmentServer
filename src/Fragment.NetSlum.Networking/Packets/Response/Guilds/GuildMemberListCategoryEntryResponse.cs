using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildMemberListCategoryEntryResponse : BaseResponse
{
    private ushort _categoryId;
    private string _categoryName = null!;

    public GuildMemberListCategoryEntryResponse SetCategoryId(ushort id)
    {
        _categoryId = id;

        return this;
    }

    public GuildMemberListCategoryEntryResponse SetCategoryName(string name)
    {
        _categoryName = name;

        return this;
    }


    public override FragmentMessage Build()
    {
        var nameBytes = _categoryName.ToShiftJis();

        var writer = new MemoryWriter(nameBytes.Length + sizeof(ushort) + 1);
        writer.Write(_categoryId);
        writer.Write(nameBytes);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildMemberListCategoryEntryResponse,
            Data = writer.Buffer,
        };
    }
}
