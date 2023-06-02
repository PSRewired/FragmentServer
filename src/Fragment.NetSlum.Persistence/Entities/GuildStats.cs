using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

public class GuildStats
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public ushort GuildId { get; set; }
    public Guild? Guild { get; set; }

    public int GoldAmount { get; set; }
    public int SilverAmount { get; set; }
    public int BronzeAmount { get; set; }
    public int CurrentGp { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Timestampable(EntityState.Modified)]
    public DateTime? UpdatedAt { get; set; }
}
