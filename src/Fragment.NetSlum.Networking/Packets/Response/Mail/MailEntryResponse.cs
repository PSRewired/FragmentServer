using System;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Mail;

public class MailEntryResponse : BaseResponse
{
    private uint _mailId;
    private uint _recipientAccountId;
    private string _recipientName = "";
    private uint _senderAccountId;
    private string _senderName = "";
    private string _subjectLine = "";
    private DateTime _sentAt = DateTime.UtcNow;

    public MailEntryResponse SetMailId(uint id)
    {
        _mailId = id;

        return this;
    }

    public MailEntryResponse SetRecipientAccountId(uint id)
    {
        _recipientAccountId = id;

        return this;
    }

    public MailEntryResponse SetRecipientName(string name)
    {
        _recipientName = name;

        return this;
    }

    public MailEntryResponse SetSenderAccountId(uint id)
    {
        _senderAccountId = id;

        return this;
    }

    public MailEntryResponse SetSenderName(string name)
    {
        _senderName = name;

        return this;
    }

    public MailEntryResponse SetSubject(string subject)
    {
        _subjectLine = subject;

        return this;
    }

    public MailEntryResponse SetSentAtDate(DateTime date)
    {
        _sentAt = date;

        return this;
    }

    public override FragmentMessage Build()
    {
        var recipientNameBytes = _recipientName.ToShiftJis(false).EnsureSize(18);
        var senderNameBytes = _senderName.ToShiftJis(false).EnsureSize(18);
        var subjectLineBytes = _subjectLine.ToShiftJis(false).EnsureSize(80);


        var writer = new MemoryWriter(sizeof(uint) * 6 +
                                      recipientNameBytes.Length +
                                      senderNameBytes.Length +
                                      subjectLineBytes.Length +
                                      1
                                      );

        writer.Write(_mailId);
        writer.Write(_recipientAccountId);
        writer.Write((uint)0);
        writer.Write((uint)_sentAt.ToEpoch());
        writer.Write((byte)0x07);
        writer.Write(_senderAccountId);
        writer.Write(senderNameBytes);
        writer.Write((uint)0);
        writer.Write(recipientNameBytes);
        writer.Write(subjectLineBytes);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataMailGetNewMailHeader,
            Data = writer.Buffer
        };
    }
}
