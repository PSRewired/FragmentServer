using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer;

public class AreaServerFavoritesCountResponse : BaseResponse
{
    private ushort _count { get; set; }

    public AreaServerFavoritesCountResponse(ushort count)
    {
        _count = count;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _count);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyFavoritesAsCount,
            Data = buffer,
        };
    }
}
