using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

public class Mail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? SenderId { get; set; }
    public PlayerAccount? Sender { get; set; }

    [MaxLength(32)]
    public string SenderName { get; set; } = "";

    public int? RecipientId { get; set; }
    public PlayerAccount? Recipient { get; set; }

    [MaxLength(32)]
    public string RecipientName { get; set; } = "";

    [MaxLength(128)]
    public string Subject { get; set; } = "";

    [MaxLength(32)]
    public string AvatarId { get; set; } = "";

    public MailContent? Content { get; set; }

    public bool Delivered { get; set; }
    public bool Read { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Timestampable(EntityState.Modified)]
    public DateTime? UpdatedAt { get; set; }
}
