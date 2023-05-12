using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyServerCategoryCountResponse : BaseResponse
{
    private readonly ushort _count;

    public LobbyServerCategoryCountResponse(ushort count)
    {
        _count = count;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _count);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyGetServersCategoryList,
            Data = buffer,
        };
    }
}
