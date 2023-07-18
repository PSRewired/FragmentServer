using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Character;
using Fragment.NetSlum.Networking.Packets.Response.Mail;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Packets.Request.Mail
{
    [FragmentPacket(MessageType.Data, OpCodes.DataMailSend)]
    public class SendMailRequest :BaseRequest
    {
        private readonly FragmentContext _database;

        public SendMailRequest(FragmentContext database)
        {
            _database = database;
        }

        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {
            var receiverAccountId = BinaryPrimitives.ReadUInt32BigEndian(request.Data[4..8].Span);
            var receiverName = request.Data.Span[8..24].ToShiftJisString();
            var senderAccountId = BinaryPrimitives.ReadUInt32BigEndian(request.Data[26..30].Span);
            var senderName = request.Data.Span[30..46].ToShiftJisString();
            var subject = request.Data.Span[48..80].ToShiftJisString();
            // Gap of just nothing between subject and body
            var body = request.Data.Span[176..1376].ToShiftJisString();
            var face = request.Data.Span[1378..1403].ToShiftJisString();
            
            IQueryable<Persistence.Entities.Character> accountLookupQuery = _database.Characters.Include(p => p.PlayerAccount);
            PlayerAccount? recipient = accountLookupQuery.FirstOrDefault(p => p.PlayerAccountId == receiverAccountId)?.PlayerAccount;
            PlayerAccount? sender = accountLookupQuery.First(p => p.PlayerAccountId == senderAccountId)?.PlayerAccount;

            if(recipient == null || sender == null)
            {
                return SingleMessage(new SendMailResponse().SetStatusCode(OpCodes.DataMailSendFailed).Build());
            }

            var mail = new Persistence.Entities.Mail()
            {
                AvatarId = face,
                Recipient = recipient,            
                RecipientName =receiverName,
                Sender = sender,
                SenderName =senderName,
                CreatedAt = DateTime.UtcNow,
                Delivered = true,
                Subject = subject,
                Read = false,
                UpdatedAt = DateTime.UtcNow,

            };

            var mailContent = new MailContent()
            {
                Content = body,
                Mail = mail
            };
            _database.Add(mail);
            _database.Add(mailContent);
            _database.SaveChanges();
            return SingleMessage(new SendMailResponse().SetStatusCode(OpCodes.DataMailSendOk).Build());
        }
    }
}
