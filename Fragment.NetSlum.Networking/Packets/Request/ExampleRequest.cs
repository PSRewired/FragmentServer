using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request;

[FragmentPacket(OpCodes.Data)]
public class ExampleRequest : BaseRequest
{
    public override Task<byte[]> GetResponse(FragmentTcpSession session, byte[] request)
    {
        throw new NotImplementedException("This is just an example.");
    }
}
