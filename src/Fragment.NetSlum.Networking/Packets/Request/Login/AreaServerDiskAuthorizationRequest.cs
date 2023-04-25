using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Login;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Login
{
    [FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerDiskAuthorizationRequest)]
    public class AreaServerIPAddressPortRequest : BaseRequest
    {
        private readonly ILogger<LogonRequest> _logger;

        public AreaServerIPAddressPortRequest(ILogger<LogonRequest> logger)
        {
            _logger = logger;
        }

        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {
            BaseResponse response = new AreaServerDiskAuthorizationResponse();
            return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
        }
    }
}
