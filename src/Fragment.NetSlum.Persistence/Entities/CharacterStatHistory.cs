using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

[Index(nameof(CreatedAt))]
public class CharacterStatHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CharacterId { get; set; }

    public Character Character { get; set; } = null!;

    public int CurrentLevel { get; set; }
    public int CurrentHp { get; set; }
    public int CurrentSp { get; set; }
    public uint CurrentGp { get; set; }
    public int OnlineTreasures { get; set; }
    public int AverageFieldLevel { get; set; }

    public int GoldAmount { get; set; }
    public int SilverAmount { get; set; }
    public int BronzeAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public static CharacterStatHistory FromStats(CharacterStats stats)
    {
        if (stats.Character == null && stats.CharacterId <= 0)
        {
            throw new DataException(
                "Cannot persist character stat history without a valid player reference. Id or entity reference must be set");
        }

        return new CharacterStatHistory
        {
            CharacterId = stats.CharacterId,
            Character = stats.Character!,
            CurrentHp = stats.CurrentHp,
            CurrentSp = stats.CurrentSp,
            CurrentGp = stats.CurrentGp,
            CurrentLevel = stats.Character?.CurrentLevel ?? 0,
            OnlineTreasures = stats.OnlineTreasures,
            AverageFieldLevel = stats.AverageFieldLevel,
            GoldAmount = stats.GoldAmount,
            SilverAmount = stats.SilverAmount,
            BronzeAmount = stats.BronzeAmount,
        };
    }
}
