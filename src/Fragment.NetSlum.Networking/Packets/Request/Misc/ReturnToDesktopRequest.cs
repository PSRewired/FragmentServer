using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Misc;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Misc;

[FragmentPacket(MessageType.Data, OpCodes.DataReturnToDesktop)]
public class ReturnToDesktopRequest : BaseRequest
{
    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        // Reset the character info when they return to desktop because they have quit the world.
        session.CharacterInfo = null;
        session.CharacterId = 0;

        return SingleMessage(new ReturnToDesktopResponse().Build());
    }
}
