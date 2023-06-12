using System;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class GetBBSUpdatesResponse:BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataBBSGetUpdatesSuccess,
            Data = new Memory<byte>(new byte[] { 0x00, 0x00 }), 
        };
    }
}