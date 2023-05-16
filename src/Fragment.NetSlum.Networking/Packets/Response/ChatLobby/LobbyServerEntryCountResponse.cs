using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyServerEntryCountResponse : BaseResponse
{
    private readonly ushort _serverCount;

    public LobbyServerEntryCountResponse(ushort serverCount)
    {
        _serverCount = serverCount;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _serverCount);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyGetServersServerList,
            Data = buffer,
        };
    }
}
