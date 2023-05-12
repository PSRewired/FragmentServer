using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System.Buffers.Binary;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class ChatLobbyEnterRoomResponse:BaseResponse
{
    private ushort _clientCount { get; set; }
    public ChatLobbyEnterRoomResponse SetClientCount(ushort count)
    {
        _clientCount = count;
        return this;
    }

    public override FragmentMessage Build()
    {

        var bufferMemory = new Memory<byte>(new byte[2]);
        var buffer = bufferMemory.Span;

        BinaryPrimitives.WriteUInt16BigEndian(buffer, _clientCount);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyEnterRoomSuccess,
            Data = bufferMemory,
        };
    }
}
