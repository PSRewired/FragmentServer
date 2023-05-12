using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Core.Extensions;
using System.Buffers.Binary;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(OpCodes.Data, OpCodes.Data_AreaServerUpdateStatusRequest)]
public class AreaServerUpdateStatusRequest :BaseRequest
{
    private readonly ILogger<AreaServerUpdateStatusRequest> _logger;

    public AreaServerUpdateStatusRequest(ILogger<AreaServerUpdateStatusRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
          
        byte[] diskId = request.Data[0..64].ToArray();
        string areaServerName = request.Data.Span[67..].ToShiftJisString();
        var pos = 67 + areaServerName.Length +1;
        session.AreaServerLevel = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[pos..(pos + 2)]);
        pos += 4;
        session.AreaServerStatus = request.Data.Span[pos];


        return Task.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
    }
}