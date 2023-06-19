using System;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Core.Extensions;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerUpdateStatusRequest)]
public class AreaServerUpdateStatusRequest :BaseRequest
{
    private readonly ILogger<AreaServerUpdateStatusRequest> _logger;

    public AreaServerUpdateStatusRequest(ILogger<AreaServerUpdateStatusRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        //byte[] diskId = request.Data[0..64].ToArray();
        var pos = 0x43;
        var serverNameBytes = request.Data[pos..].Span.ReadToNullByte();
        pos += serverNameBytes.Length + 1;
        session.AreaServerInfo!.ServerName = serverNameBytes.ToShiftJisString();
        session.AreaServerInfo!.Level = BinaryPrimitives.ReadUInt16BigEndian(request.Data[pos..(pos + 2)].Span);
        //pos + 2 is some sort of status flag
        pos += 3;
        session.AreaServerInfo!.State = request.Data.Span[pos++];
        session.AreaServerInfo!.Detail = request.Data[pos..];

        _logger.LogDebug("Area server status update:\n{Details}", session.AreaServerInfo.ToString());

        return NoResult();
    }
}
