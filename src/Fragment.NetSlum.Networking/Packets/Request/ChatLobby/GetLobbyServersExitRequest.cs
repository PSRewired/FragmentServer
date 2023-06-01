using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(OpCodes.Data, OpCodes.DataLobbyGetServersExit)]
public class GetLobbyServersExitRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        return Task.FromResult<ICollection<FragmentMessage>>(new[] { new LobbyGetServersResponse().Build() });
    }
}