using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Core.Extensions;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Events;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerUpdateStatusRequest)]
public class AreaServerUpdateStatusRequest : BaseRequest
{
    private readonly ILogger<AreaServerUpdateStatusRequest> _logger;
    private readonly ICommandBus _commandBus;

    public AreaServerUpdateStatusRequest(ILogger<AreaServerUpdateStatusRequest> logger, ICommandBus commandBus)
    {
        _logger = logger;
        _commandBus = commandBus;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        //byte[] diskId = request.Data[0..64].ToArray();
        var pos = 0x43;
        var serverNameBytes = request.Data[pos..].Span.ReadToNullByte();
        pos += serverNameBytes.Length + 1;

        var serverName = serverNameBytes.ToShiftJisString();
        var level = BinaryPrimitives.ReadUInt16BigEndian(request.Data[pos..(pos + 2)].Span);
        pos += 3;
        var status = (AreaServerStatus)request.Data.Span[pos++];
        var state = (AreaServerState)request.Data.Span[pos++];

        // Kinda jank, but punting.
        if (!serverName.Equals(session.AreaServerInfo!.ServerName, System.StringComparison.InvariantCultureIgnoreCase)
            || level != session.AreaServerInfo!.Level
            || status != session.AreaServerInfo!.Status
            || state != session.AreaServerInfo!.State
           )
        {
            _commandBus.Notify(new AreaServerStatusUpdateEvent(serverName, level, status, state, session.AreaServerInfo.CurrentPlayerCount)).Wait();
        }

        session.AreaServerInfo!.ServerName = serverName;
        session.AreaServerInfo!.Level = level;
        //pos + 2 is some sort of status flag
        session.AreaServerInfo!.Status = status;
        session.AreaServerInfo!.State = state;
        session.AreaServerInfo!.ServerId = request.Data[pos..];

        _logger.LogDebug("Area server status update:\n{Details}", session.AreaServerInfo.ToString());

        return NoResponse();
    }
}
