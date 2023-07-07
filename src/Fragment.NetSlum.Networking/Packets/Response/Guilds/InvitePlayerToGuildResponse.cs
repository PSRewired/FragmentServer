using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class InvitePlayerToGuildResponse : BaseResponse
{
    private readonly ushort _playerIndex;

    public InvitePlayerToGuildResponse(ushort playerIndex)
    {
        _playerIndex = playerIndex;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span[..2], _playerIndex);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataInvitePlayerToGuildResponse,
            Data = buffer,
        };
    }
}
