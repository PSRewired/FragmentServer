using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class CreateLobbyChatroomResponse : BaseResponse
{
    private readonly ushort _lobbyId;

    public CreateLobbyChatroomResponse(ushort lobbyId)
    {
        _lobbyId = lobbyId;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _lobbyId);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyChatroomCreateOk,
            Data = buffer,
        };
    }
}
