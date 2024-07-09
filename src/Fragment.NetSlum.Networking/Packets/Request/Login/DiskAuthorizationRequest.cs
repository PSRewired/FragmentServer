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

[FragmentPacket(MessageType.Data, OpCodes.DataDiskAuthorizationRequest)]
public class DiskAuthorizationRequest : BaseRequest
{
    private readonly ILogger<DiskAuthorizationRequest> _logger;

    public DiskAuthorizationRequest(ILogger<DiskAuthorizationRequest> logger)
    {
        _logger = logger;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response = new DiskAuthorizationResponse();

        return SingleMessage(response.Build());
    }
}
