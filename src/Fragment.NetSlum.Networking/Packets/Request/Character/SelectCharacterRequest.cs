using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Character;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Character;

[FragmentPacket(OpCodes.Data, OpCodes.DataSelectCharRequest)]
public class SelectCharacterRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        return Task.FromResult<ICollection<FragmentMessage>>(new [] {new SelectCharacterResponse().Build()});
    }
}
