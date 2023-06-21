using System;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System.Buffers.Binary;
using Fragment.NetSlum.Core.Buffers;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyEventResponse:BaseResponse
{
    private Memory<byte> _data;
    private ushort _senderIndex;
    private bool _isSender;
    public LobbyEventResponse SetData(Memory<byte> status)
    {
        _data = status;
        return this;
    }

    public LobbyEventResponse SetSenderIndex(ushort index)
    {
        _senderIndex = index;
        return this;
    }
    public LobbyEventResponse SetIsSender(bool isSender)
    {
        _isSender = isSender;
        return this;
    }


    public override FragmentMessage Build()
    {
        var expectedIndex = _isSender ? (ushort)0xFFFF : _senderIndex;
        var writer = new MemoryWriter(_data.Length + sizeof(ushort));

        writer.Write((byte)(expectedIndex >> 8));
        writer.Write((byte)(expectedIndex & 0xFF));
        writer.Write(_data);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyEvent,
            Data = writer.Buffer,
        };
    }
}
