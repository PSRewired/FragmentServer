using System;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class ChatLobbyStatusUpdateResponse:BaseResponse
{
    private ushort _playerIndex;
    private Memory<byte> _lastStatus = Array.Empty<byte>();

    public ChatLobbyStatusUpdateResponse SetPlayerIndex(ushort playerIndex)
    {
        _playerIndex = playerIndex;

        return this;
    }

    public ChatLobbyStatusUpdateResponse SetLastStatus(Memory<byte> status)
    {
        _lastStatus = status;
        return this;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(ushort) * 2 + _lastStatus.Length);
        writer.Write(_playerIndex);
        writer.Write((ushort)_lastStatus.Length);
        writer.Write(_lastStatus);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyStatusUpdate,
            Data = writer.Buffer,
        };
    }
}
