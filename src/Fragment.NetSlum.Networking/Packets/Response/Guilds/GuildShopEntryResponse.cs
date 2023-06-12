using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildShopEntryResponse : BaseResponse
{
    private ushort _guildId;
    private string _guildName = null!;

    public GuildShopEntryResponse SetGuildId(ushort id)
    {
        _guildId = id;

        return this;
    }

    public GuildShopEntryResponse SetGuildName(string name)
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
            DataPacketType = OpCodes.DataGuildShopEntryResponse,
            Data = writer.Buffer,
        };
    }
}
