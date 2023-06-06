using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyServerCategoryEntryResponse : BaseResponse
{
    private ushort _id;
    private string _name = "";

    public LobbyServerCategoryEntryResponse SetCategoryId(ushort id)
    {
        _id = id;

        return this;
    }

    public LobbyServerCategoryEntryResponse SetCategoryName(string name)
    {
        _name = name;

        return this;
    }

    public override FragmentMessage Build()
    {

        var nameBytes = _name.ToShiftJis();
        var writer = new MemoryWriter(nameBytes.Length + sizeof(ushort));
        writer.Write(_id);
        writer.Write(nameBytes);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyGetServersEntryCategory,
            Data = writer.Buffer,
        };
    }
}
