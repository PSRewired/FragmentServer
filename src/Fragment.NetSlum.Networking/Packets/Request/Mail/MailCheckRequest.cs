using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Mail;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Mail;

[FragmentPacket(OpCodes.Data, OpCodes.DataMailCheckRequest)]
public class MailCheckRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var accountId = BinaryPrimitives.ReadInt32BigEndian(request.Data.Span[..4]);

        return Task.FromResult<ICollection<FragmentMessage>>(new[] { new MailCheckResponse()
            .SetHasMail(true)
            .Build() });
    }
}
