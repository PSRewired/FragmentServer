using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Packets.Response.Login;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Login;

[FragmentPacket(MessageType.Data, OpCodes.Data_LogonRequest)]
public class LogonRequest : BaseRequest
{
    private readonly ILogger<LogonRequest> _logger;

    public LogonRequest(ILogger<LogonRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response;

        switch (request.Data.Span[1])
        {
            case (byte)OpCodes.DataServerKeyChange:
                _logger.LogInformation("Session {SessionId} has identified itself as an Area Server", session.Id);
                session.AreaServerInfo = new AreaServerInformation();
                response = new AreaServerLogonResponse();
                break;
            default:
                _logger.LogInformation("Session {SessionId} has identified itself as a Game Client", session.Id);
                response = new LogonResponse();
                break;
        }

        return SingleMessage(response.Build());
    }
}
