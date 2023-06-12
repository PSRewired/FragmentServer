using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class ReassignGuildMasterResponse : BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataGuildReassignMasterResponse,
            Data = new Memory<byte>(new byte[2]),
        };
    }
}