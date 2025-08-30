using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Request;
using Fragment.NetSlum.Networking.Packets.Response.Misc;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

/// <summary>
/// This packet is called after successfully donating an item to a guild shop. Unsure of what its actual usage is for.
/// </summary>
[FragmentPacket(MessageType.Data, OpCodes.DataUnknown787b)]
public class Unknown787bRequest : BaseRequest
{
    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        return SingleMessage(new UnknownResponse(OpCodes.DataUnknown787cResponse).Build());
    }
}
