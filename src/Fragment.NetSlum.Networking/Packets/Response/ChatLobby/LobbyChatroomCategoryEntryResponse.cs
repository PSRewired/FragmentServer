using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyChatroomCategoryEntryResponse : BaseResponse
{
    private ushort _id;
    private string _name = "";
    private ushort _currentPlayerCount;

    public LobbyChatroomCategoryEntryResponse SetCategoryId(ushort id)
    {
        _id = id;

        return this;
    }

    public LobbyChatroomCategoryEntryResponse SetCategoryName(string name)
    {
        _name = name;

        return this;
    }

    public LobbyChatroomCategoryEntryResponse SetCurrentPlayerCount(ushort playerCount)
    {
        _currentPlayerCount = playerCount;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _name.ToShiftJis();
        var writer = new MemoryWriter(nameBytes.Length + sizeof(ushort) * 30);
        writer.Write(_id);
        writer.Write(nameBytes);
        writer.Write((ushort)1);
        writer.Write((ushort)1);
        writer.Write(_currentPlayerCount);
        writer.Write((ushort)0xFFFF); // Lobby status: 1/2/3 - Closed/Error, 4/5/6/7 - Password Required
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);
        writer.Write((ushort)0xFFFF);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyChatroomListError,
            Data = writer.Buffer,
        };
    }
}
