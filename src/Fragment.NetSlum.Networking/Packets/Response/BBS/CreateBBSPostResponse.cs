using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class CreateBBSPostResponse : BaseResponse
{
    public override FragmentMessage Build() => new FragmentMessage
    {
        OpCode = OpCodes.Data,
        DataPacketType = OpCodes.DataBbsCreatePostResponse,
        Data = new Memory<byte>(new byte[2]),
    };
}
