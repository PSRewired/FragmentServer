using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Login;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using System.Net;
using Fragment.NetSlum.Networking.Extensions;
using System.Buffers.Binary;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer
{
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
           
            IPAddress areaServerIp = IPAddress.Parse(request.Data[0..4].ToArray().BuildIPStringFromBytes());
            ushort areaServerPort = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[4..6]);
            BaseResponse response = new AreaServerIPAddressPortResponse();
            return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
        }
    }
}
