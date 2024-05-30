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
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.Mail;

[FragmentPacket(MessageType.Data, OpCodes.DataGetMailList)]
public class GetMailListRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetMailListRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var accountId = BinaryPrimitives.ReadUInt32BigEndian(request.Data.Span[..4]);

        var availableMail = _database.Mails
            .AsNoTracking()
            .Where(m => m.RecipientId == accountId && m.Delivered == false);

        var responses = new List<FragmentMessage>
        {
            new MailListCountResponse((uint)availableMail.Count()).Build(),
        };

        foreach (var mailPiece in availableMail)
        {
            responses.Add(new MailEntryResponse()
                .SetMailId((uint)mailPiece.Id)
                .SetSubject(mailPiece.Subject)
                .SetSenderName(mailPiece.SenderName)
                .SetSenderAccountId((uint)(mailPiece.SenderId ?? 0))
                .SetRecipientName(mailPiece.RecipientName)
                .SetRecipientAccountId((uint)(mailPiece.RecipientId ?? 0))
                .SetSentAtDate(mailPiece.CreatedAt)
                .Build());

            // Set the mail as delivered
            mailPiece.Delivered = true;
            _database.Entry(mailPiece).State = EntityState.Modified;
        }

        _database.SaveChanges();

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
