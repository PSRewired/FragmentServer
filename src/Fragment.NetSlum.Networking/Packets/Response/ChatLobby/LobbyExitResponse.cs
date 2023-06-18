using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby
{
    public class LobbyExitResponse:BaseResponse
    {
        public override FragmentMessage Build()
        {
            return new FragmentMessage
            {
                OpCode = OpCodes.Data,
                DataPacketType = OpCodes.DataLobbyExitRoomOk,
                Data = new byte[] { 0x00,0x00 }
            };
        }
    }
}
