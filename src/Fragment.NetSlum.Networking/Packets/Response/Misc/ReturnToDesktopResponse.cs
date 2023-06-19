using System;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Misc;

public class ReturnToDesktopResponse : BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataReturnToDesktopResponse,
            Data = new Memory<byte>(new byte[2]),
        };
    }
}
