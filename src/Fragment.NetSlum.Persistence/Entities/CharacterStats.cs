using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

[Index(nameof(UpdatedAt))]
public class CharacterStats
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CharacterId { get; set; }
    public Character? Character { get; set; }

    public int CurrentHp { get; set; }
    public int CurrentSp { get; set; }
    public uint CurrentGp { get; set; }
    public int OnlineTreasures { get; set; }
    public int AverageFieldLevel { get; set; }

    public int GoldAmount { get; set; }
    public int SilverAmount { get; set; }
    public int BronzeAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Timestampable(EntityState.Modified)]
    public DateTime? UpdatedAt { get; set; }
}
