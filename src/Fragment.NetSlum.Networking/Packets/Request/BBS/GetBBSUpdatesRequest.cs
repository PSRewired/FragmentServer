using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Networking.Packets.Response.BBS;

namespace Fragment.NetSlum.Networking.Packets.Request.BBS
{
    [FragmentPacket(OpCodes.Data, OpCodes.Data_BBSGetUpdatesRequest)]
    public class GetBBSUpdatesRequest:BaseRequest
    {
        private readonly ILogger<GetBBSUpdatesRequest> _logger;

        public GetBBSUpdatesRequest(ILogger<GetBBSUpdatesRequest> logger)
        {
            _logger = logger;
        }

        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {
            BaseResponse response = new GetBBSUpdatesResponse();

            return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
        }
    }
}
