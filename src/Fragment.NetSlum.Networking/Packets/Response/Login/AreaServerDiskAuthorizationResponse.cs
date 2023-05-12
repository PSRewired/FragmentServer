using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Login;

public class AreaServerDiskAuthorizationResponse : BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.Data_AreaServerDiskAuthorizationSuccess,
            Data = new byte[] { 0x00, 0x00 },
        };
    }
}