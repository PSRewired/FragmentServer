using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerIpPortRequest)]
public class AreaServerIPAddressPortRequest : BaseRequest
{
    private readonly ILogger<AreaServerIPAddressPortRequest> _logger;

    public AreaServerIPAddressPortRequest(ILogger<AreaServerIPAddressPortRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var ipAddressBytes = new Span<byte>(new byte[4]);
        request.Data.Span[..4].CopyTo(ipAddressBytes);
        ipAddressBytes.Reverse();

        session.AreaServerInfo!.ConnectionEndpoint = new IPEndPoint(
            new IPAddress(ipAddressBytes.ToArray()), BinaryPrimitives.ReadUInt16BigEndian(request.Data[4..6].Span));

        BaseResponse response = new AreaServerIPAddressPortResponse();
        return SingleMessage(response.Build());
    }
}
