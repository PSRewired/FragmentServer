using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class ClientLeftChatLobbyResponse : BaseResponse
{
    private readonly ushort _clientIdx;

    public ClientLeftChatLobbyResponse(ushort clientIdx)
    {
        _clientIdx = clientIdx;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _clientIdx);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.ClientLeavingLobby,
            Data = buffer,
        };
    }
}
