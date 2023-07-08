using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyChatroomCategoryEntryResponse : BaseResponse
{
    private readonly OpCodes _entryType;
    private ushort _id;
    private string _name = "";
    private ushort _currentPlayerCount;
    private ChatLobbyStatus _lobbyStatus = ChatLobbyStatus.Active;
    private bool _isCreationEntry;
    private bool _requiresPassword;

    public LobbyChatroomCategoryEntryResponse(OpCodes entryType)
    {
        _entryType = entryType;
    }

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

    public LobbyChatroomCategoryEntryResponse SetLobbyStatus(ChatLobbyStatus status)
    {
        _lobbyStatus = status;

        return this;
    }

    public LobbyChatroomCategoryEntryResponse SetIsCreationEntry(bool creation)
    {
        _isCreationEntry = creation;

        return this;
    }

    public LobbyChatroomCategoryEntryResponse SetPasswordRequired(bool required)
    {
        _requiresPassword = required;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _name.ToShiftJis();
        var writer = new MemoryWriter(nameBytes.Length + sizeof(ushort) * 5);
        writer.Write(_id);
        writer.Write(nameBytes);
        writer.Write((ushort)(_requiresPassword ? 1 : 0)); // requires password 1/0
        writer.Write((ushort)(_isCreationEntry ? 0 : 1)); // 0 - List entry to create a new chat lobby -- 1 - Already created password lobby
        //writer.Write((ushort)1);
        writer.Write(_currentPlayerCount);
        writer.Write((ushort) _lobbyStatus); // Lobby status: 1/2/3 - Closed/Error, 4/5/6/7 - Password Required

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = _entryType,
            Data = writer.Buffer,
        };
    }
}
