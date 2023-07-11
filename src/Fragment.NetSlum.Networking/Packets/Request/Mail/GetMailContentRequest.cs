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

[FragmentPacket(MessageType.Data, OpCodes.DataGetMailContent)]
public class GetMailContentRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetMailContentRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var accountId = BinaryPrimitives.ReadUInt32BigEndian(request.Data[..4].Span);
        var mailId = BinaryPrimitives.ReadUInt32BigEndian(request.Data[4..8].Span);

        var mail = _database.Mails
            .AsNoTracking()
            .Include(m => m.Content)
            .First(m => m.Id == mailId && m.RecipientId == accountId);

        return SingleMessage(new MailContentResponse()
            .SetContent(mail.Content?.Content ?? "<NONE>")
            .SetAvatarDescriptor(mail.AvatarId)
            .Build());
    }
}
