using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("mail_body")]
[Index("MailId", Name = "MAIL_BODY_MAIL_ID_uindex", IsUnique = true)]
[MySqlCharSet("sjis")]
[MySqlCollation("sjis_japanese_ci")]
public class MailBody
{
    [Key]
    [Column("MAIL_BODY_ID")]
    public int MailBodyId { get; set; }

    [Column("MAIL_ID")]
    public int MailId { get; set; }

    [Column("MAIL_BODY", TypeName = "blob")]
    public byte[]? MailBody1 { get; set; }

    [Column("FACE_ID")]
    [StringLength(30)]
    [MySqlCharSet("utf8mb4")]
    [MySqlCollation("utf8mb4_0900_ai_ci")]
    public string FaceId { get; set; } = null!;

    [ForeignKey("MailId")]
    [InverseProperty("MailBody")]
    public virtual MailMetum Mail { get; set; } = null!;
}
