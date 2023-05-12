using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerIpPortRequest)]
public class AreaServerIPAddressPortRequest : BaseRequest
{
    private readonly ILogger<AreaServerIPAddressPortRequest> _logger;

    public AreaServerIPAddressPortRequest(ILogger<AreaServerIPAddressPortRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        session.IpAddress = request.Data[0..4].ToArray();
        session.Port = request.Data[4..6].ToArray();
        BaseResponse response = new AreaServerIPAddressPortResponse();
        return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
    }
}