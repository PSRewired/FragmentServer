using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer;

public class AreaServerPublishResponse :BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.Data_AreaServerPublishSuccess,
            Data = new byte[] { 0x00, 0x00 },
        };
    }
}