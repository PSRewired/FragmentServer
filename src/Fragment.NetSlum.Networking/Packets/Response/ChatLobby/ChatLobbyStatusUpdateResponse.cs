using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class ChatLobbyStatusUpdateResponse:BaseResponse
{
    private ushort? _playerIndex;
    private Memory<byte> _lastStatus = Array.Empty<byte>();

    public ChatLobbyStatusUpdateResponse SetPlayerIndex(ushort playerIndex)
    {
        _playerIndex = playerIndex;

        return this;
    }

    //public ChatLobbyStatusUpdateResponse SetLastStatus(Memory<byte> status)
    //{
    //    _lastStatus = status;
    //    return this;
    //}
    public ChatLobbyStatusUpdateResponse SetLastStatus(Memory<byte> status)
    {
        _lastStatus = status;
        return this;
    }

    public override FragmentMessage Build()
    {
        //var size = _lastStatus.Length;

        //if (_playerIndex != null)
        //{
        //    size += sizeof(ushort) * 2;
        //}

        //var buffer = new Memory<byte>(new byte[size]);
        //var bufferSpan = buffer.Span;

        //var pos = 0;

        //if (_playerIndex != null)
        //{
        //    BinaryPrimitives.WriteUInt16BigEndian(bufferSpan, (ushort)(_playerIndex + 1));
        //    BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[2..], (ushort)_lastStatus.Length);
        //    pos += 4;
        //}

        //_lastStatus.CopyTo(buffer[pos..]);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyStatusUpdate,
            Data = _lastStatus,
        };
    }
}
