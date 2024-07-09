using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Mail;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.Mail;

[FragmentPacket(MessageType.Data, OpCodes.DataMailCheckRequest)]
public class MailCheckRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public MailCheckRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var accountId = BinaryPrimitives.ReadInt32BigEndian(request.Data.Span[..4]);

        var undeliveredMailCount = _database.Mails
            .Count(m => m.RecipientId == accountId && m.Delivered == false);

        //await _database.Mails
        //    .Where(m => m.RecipientId == accountId && m.Delivered == false)
        //    .ExecuteUpdateAsync(m => m.SetProperty(p => p.Delivered, v => true));

        return SingleMessage(new MailCheckResponse()
            .SetHasMail((uint)undeliveredMailCount)
            .Build());
    }
}
