using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerPublishDetails1Request)]
[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerPublishDetails2Request)]
[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerPublishDetails3Request)]
[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerPublishDetails4Request)]
[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerPublishDetails6Request)]
public class AreaServerPublishDetailsRequest:BaseRequest
{
    private readonly ILogger<AreaServerPublishDetailsRequest> _logger;

    public AreaServerPublishDetailsRequest(ILogger<AreaServerPublishDetailsRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response;

        switch (request.DataPacketType)
        {
            case OpCodes.Data_AreaServerPublishDetails1Request:
                byte[] diskId = request.Data[0..64].ToArray();
                session.AreaServerName = request.Data[65..79].ToArray();
                response = new AreaServerPublishDetailsResponse() { PacketType = OpCodes.Data_AreaServerPublishDetails1Success, Data = new byte[] { 0x00, 0x01 } };
                break;
            case OpCodes.Data_AreaServerPublishDetails2Request:
                response = new AreaServerPublishDetailsResponse() { PacketType = OpCodes.Data_AreaServerPublishDetails2Success, Data = new byte[] { 0xDE, 0xAD } };
                break;

            case OpCodes.Data_AreaServerPublishDetails3Request:
            case OpCodes.Data_AreaServerPublishDetails4Request:
            case OpCodes.Data_AreaServerPublishDetails6Request:
            default:
                return Task.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
        }


        return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
    }
}
