using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Login
{
    public class DiskAuthorizationResponse :BaseResponse
    {
        public override FragmentMessage Build()
        {
            return new FragmentMessage
            {
                OpCode = OpCodes.Data,
                DataPacketType = OpCodes.Data_DiskAuthorizationSuccess,
                Data = new byte[] { 0x78, 0x94 },
            };
        }
    }
}
