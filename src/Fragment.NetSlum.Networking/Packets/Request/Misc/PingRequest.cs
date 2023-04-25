using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Packets.Response.Misc;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Misc;

[FragmentPacket(OpCodes.Ping)]
[FragmentPacket(OpCodes.Data, OpCodes.DataPing)]
public class PingRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response = new PingResponse();

        return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
      
    }
}
