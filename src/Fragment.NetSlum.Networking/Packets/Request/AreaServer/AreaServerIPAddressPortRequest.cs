using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;
using Fragment.NetSlum.TcpServer.Extensions;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerIpPortRequest)]
public class AreaServerIPAddressPortRequest : BaseRequest
{
    private readonly ILogger<AreaServerIPAddressPortRequest> _logger;

    public AreaServerIPAddressPortRequest(ILogger<AreaServerIPAddressPortRequest> logger)
    {
        _logger = logger;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var ipAddressBytes = new Span<byte>(new byte[4]);
        request.Data.Span[..4].CopyTo(ipAddressBytes);
        ipAddressBytes.Reverse();

        var asIpAddress = new IPAddress(ipAddressBytes.ToArray());
        var asPort = BinaryPrimitives.ReadUInt16BigEndian(request.Data[4..6].Span);

        if (asIpAddress.IsPrivate())
        {
            var socketIp = IPAddress.Parse(session.Socket!.GetClientIp());
            _logger.LogWarning("Area server {ServerName} sent a private IP of {PrivateIp}. Attempting to override using their socket IP of {SocketIp}", session.AreaServerInfo!.ServerName, asIpAddress, socketIp);
            session.AreaServerInfo!.PrivateConnectionEndpoint = new IPEndPoint(asIpAddress, asPort);
            asIpAddress = socketIp;
        }

        session.AreaServerInfo!.PublicConnectionEndpoint = new IPEndPoint(
            asIpAddress, BinaryPrimitives.ReadUInt16BigEndian(request.Data[4..6].Span));

        _logger.LogInformation("Area Server Published Connection Info: {NewLine}{AreaServerInfo}", Environment.NewLine, session.AreaServerInfo!.ToString());

        BaseResponse response = new AreaServerIPAddressPortResponse();
        return SingleMessage(response.Build());
    }
}
