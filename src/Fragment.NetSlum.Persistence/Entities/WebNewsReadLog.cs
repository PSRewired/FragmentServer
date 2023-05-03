using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class WebNewsReadLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    public int PlayerAccountId { get; set; }
    public PlayerAccount PlayerAccount { get; set; } = null!;

    public ushort WebNewsArticleId { get; set; }
    public WebNewsArticle WebNewsArticle { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
