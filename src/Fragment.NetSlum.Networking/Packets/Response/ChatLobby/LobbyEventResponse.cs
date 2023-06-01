using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System.Buffers.Binary;

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
        var bufferMemory = new Memory<byte>(new byte[_data.Length]);
        var buffer = bufferMemory.Span;

        _data.CopyTo(bufferMemory);

        BinaryPrimitives.WriteUInt16BigEndian(buffer[..2], !_isSender ? _senderIndex : (ushort)0xFFFF);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyEvent,
            Data = bufferMemory,
        };
    }
}