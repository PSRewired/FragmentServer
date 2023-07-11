using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class MailContent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int MailId { get; set; }
    public Mail Mail { get; set; } = null!;

    [MaxLength(1200)]
    public string Content { get; set; } = "";
}
