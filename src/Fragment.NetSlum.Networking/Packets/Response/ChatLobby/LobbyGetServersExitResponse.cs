using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyGetServersExitResponse : BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyGetServersExitOk,
            Data = new byte[2],
        };
    }
}
