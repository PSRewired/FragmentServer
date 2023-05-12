using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System.Buffers.Binary;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class ChatLobbyCountResponse:BaseResponse
{
    private ushort _chatLobbyCount;
    public ChatLobbyCountResponse SetChatLobbyCount(ushort count)
    {
        _chatLobbyCount = count;

        return this;
    }


    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        var bufferSpan = buffer.Span;

        BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[0..], _chatLobbyCount);
        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyLobbyList,
            Data =buffer,
        };
    }
}