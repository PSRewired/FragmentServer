using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Events;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Misc;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.TcpServer.Extensions;

namespace Fragment.NetSlum.Networking.Packets.Request.Misc;

[FragmentPacket(MessageType.Data, OpCodes.DataReturnToDesktop)]
public class ReturnToDesktopRequest : BaseRequest
{
    private readonly ICommandBus _commandBus;

    public ReturnToDesktopRequest(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var currentCharacterId = session.CharacterId;

        // Reset the character info when they return to desktop because they have quit the world.
        session.CharacterInfo = null;
        session.CharacterId = 0;

        await _commandBus.Notify(new CharacterLoggedOutEvent(currentCharacterId, session.Socket!.GetClientIp()));

        return SingleMessageAsync(new ReturnToDesktopResponse().Build());
    }
}
