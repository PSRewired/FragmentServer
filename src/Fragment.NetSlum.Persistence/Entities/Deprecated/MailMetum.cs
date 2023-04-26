using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("mail_meta")]
[Index("MailId", Name = "MAIL_META_MAIL_ID_uindex", IsUnique = true)]
[MySqlCharSet("sjis")]
[MySqlCollation("sjis_japanese_ci")]
public class MailMetum
{
    [Key]
    [Column("MAIL_ID")]
    public int MailId { get; set; }

    [Column("RECEIVER_ACCOUNT_ID")]
    public int ReceiverAccountId { get; set; }

    [Column("DATE", TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column("SENDER_ACCOUNT_ID")]
    public int SenderAccountId { get; set; }

    [Column("SENDER_NAME", TypeName = "blob")]
    public byte[]? SenderName { get; set; }

    [Column("RECEIVER_NAME", TypeName = "blob")]
    public byte[] ReceiverName { get; set; } = null!;

    [Column("MAIL_SUBJECT", TypeName = "blob")]
    public byte[] MailSubject { get; set; } = null!;

    [Column("MAIL_DELIVERED")]
    public bool? MailDelivered { get; set; }

    [InverseProperty("Mail")]
    public virtual MailBody? MailBody { get; set; }
}
