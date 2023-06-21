using System.Buffers.Binary;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails1Request)]
[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails2Request)]
[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails3Request)]
[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails4Request)]
[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails6Request)]
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
                session.AreaServerInfo!.DiskId = request.Data[..0x40].Span.GetNullTerminatedString();

                var nameBytes = request.Data[0x41..].Span.ReadToNullByte();
                session.AreaServerInfo!.ServerName = nameBytes.ToShiftJisString();

                var pos = 0x41 + nameBytes.Length + 1;

                session.AreaServerInfo!.Level = BinaryPrimitives.ReadUInt16BigEndian(request.Data[pos..(pos + 2)].Span);
                pos += 3;
                session.AreaServerInfo!.State = request.Data.Span[pos++];
                session.AreaServerInfo!.CurrentPlayerCount = BinaryPrimitives.ReadUInt16BigEndian(request.Data[pos..(pos + 2)].Span);
                pos += 3;
                session.AreaServerInfo.Detail = request.Data[pos..];

                response = new AreaServerPublishDetailsResponse { PacketType = OpCodes.Data_AreaServerPublishDetails1Success, Data = new byte[] { 0x00, 0x01 } };
                break;
            case OpCodes.Data_AreaServerPublishDetails2Request:
                response = new AreaServerPublishDetailsResponse { PacketType = OpCodes.Data_AreaServerPublishDetails2Success, Data = new byte[] { 0xDE, 0xAD } };
                break;

            case OpCodes.Data_AreaServerPublishDetails3Request:
                response = new AreaServerPublishDetailsResponse { PacketType = OpCodes.Data_AreaServerPublishDetails3Success, Data = new byte[] { 0x00, 0x01 } };
                break;
            case OpCodes.Data_AreaServerPublishDetails4Request:
                response = new AreaServerPublishDetailsResponse { PacketType = OpCodes.Data_AreaServerPublishDetails4Success, Data = new byte[] { 0x00, 0x01 } };
                break;
            case OpCodes.Data_AreaServerPublishDetails6Request:
                //response = new AreaServerPublishDetailsResponse() { PacketType = OpCodes.Data_AreaServerPublishDetails6Success, Data = new byte[] { 0x00, 0x09 } };
            default:
                return NoResult();
        }

        return ReturnSingle(response.Build());
    }
}
