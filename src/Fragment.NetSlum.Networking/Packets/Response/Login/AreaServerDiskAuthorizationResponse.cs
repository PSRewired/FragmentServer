using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Packets.Response.Login
{
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
}
