using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Login
{
    public class AreaServerLogon2Response : BaseResponse
    {
        public override FragmentMessage Build()
        {
            return new FragmentMessage
            {
                OpCode = OpCodes.Data,
                DataPacketType = OpCodes.DataAreaServerLogon2Response,
                Data = new byte[] { 0x02, 0x11 },
            };
        }
    }
}
