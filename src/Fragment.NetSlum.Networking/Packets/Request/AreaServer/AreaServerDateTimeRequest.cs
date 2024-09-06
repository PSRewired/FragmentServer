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

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerDateTimeRequest)]
public class AreaServerDateTimeRequest : BaseRequest
{
    private readonly ILogger<AreaServerIPAddressPortRequest> _logger;

    public AreaServerDateTimeRequest(ILogger<AreaServerIPAddressPortRequest> logger)
    {
        _logger = logger;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response = new AreaServerDateTimeResponse();
        return SingleMessage(response.Build());
    }
}
