using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer
{
    public class AreaServerPublishResponse :BaseResponse
    {
        public override FragmentMessage Build()
        {
            return new FragmentMessage
            {
                OpCode = OpCodes.Data,
                DataPacketType = OpCodes.Data_AreaServerPublishSucess,
                Data = new byte[] { 0x00, 0x00 },
            };
        }
    }
}
