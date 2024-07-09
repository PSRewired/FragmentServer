using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Packets.Response.Login;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Login;

[FragmentPacket(MessageType.Data, OpCodes.AreaServerShutdownRequest)]
public class AreaServerShutdownRequest : BaseRequest
{
    private readonly ILogger<AreaServerShutdownRequest> _logger;

    public AreaServerShutdownRequest(ILogger<AreaServerShutdownRequest> logger)
    {
        _logger = logger;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response = new AreaServerShutdownResponse();

        return SingleMessage(response.Build());
    }
}
