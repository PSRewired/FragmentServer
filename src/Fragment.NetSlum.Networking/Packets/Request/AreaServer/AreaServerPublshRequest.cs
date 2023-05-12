using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerPublishRequest)]
public class AreaServerPublshRequest :BaseRequest
{
    private readonly ILogger<AreaServerPublshRequest> _logger;

    public AreaServerPublshRequest(ILogger<AreaServerPublshRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response = new AreaServerPublishResponse();
        return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
    }
}