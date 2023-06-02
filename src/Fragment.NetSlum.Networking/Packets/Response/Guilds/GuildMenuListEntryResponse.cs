using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildMenuListEntryResponse : BaseResponse
{
    private ushort _guildId;
    private string _guildName = null!;

    public GuildMenuListEntryResponse SetGuildId(ushort id)
    {
        _guildId = id;

        return this;
    }

    public GuildMenuListEntryResponse SetGuildName(string name)
    {
        _guildName = name;

        return this;
    }


    public override FragmentMessage Build()
    {
        var nameBytes = _guildName.ToShiftJis();

        var writer = new MemoryWriter(nameBytes.Length + sizeof(ushort) + 1);
        writer.Write(_guildId);
        writer.Write(nameBytes);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.Data_GuildListItemResponse,
            Data = writer.Buffer,
        };
    }
}
