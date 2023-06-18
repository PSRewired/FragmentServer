using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Login;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Login;

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerDiskAuthorizationRequest)]
public class AreaServerDiskAuthorizationRequset : BaseRequest
{
    private readonly ILogger<AreaServerDiskAuthorizationRequset> _logger;

    public AreaServerDiskAuthorizationRequset(ILogger<AreaServerDiskAuthorizationRequset> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response = new AreaServerDiskAuthorizationResponse();
        return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
    }
}
