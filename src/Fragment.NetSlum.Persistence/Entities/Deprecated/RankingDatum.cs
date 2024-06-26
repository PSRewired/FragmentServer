﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("ranking_data")]
public class RankingDatum
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("AntiCheatEngineResult", TypeName = "tinytext")]
    public string AntiCheatEngineResult { get; set; } = null!;

    [Column("loginTime", TypeName = "text")]
    public string LoginTime { get; set; } = null!;

    [Column("DiskID", TypeName = "text")]
    public string DiskId { get; set; } = null!;

    [Column("SaveID", TypeName = "text")]
    public string SaveId { get; set; } = null!;

    [Column("CharacterSaveID", TypeName = "text")]
    public string CharacterSaveId { get; set; } = null!;

    [Column("CharacterName", TypeName = "text")]
    public string CharacterName { get; set; } = null!;

    [Column("CharacterLevel")]
    public byte CharacterLevel { get; set; }

    [Column("CharacterClassName", TypeName = "text")]
    public string CharacterClassName { get; set; } = null!;

    [Column("CharacterHP")]
    public ushort CharacterHp { get; set; }

    [Column("CharacterSP")]
    public ushort CharacterSp { get; set; }

    [Column("CharacterGP", TypeName = "mediumint unsigned")]
    public uint CharacterGp { get; set; }

    [Column("GodStatueCounterOnline", TypeName = "mediumint unsigned")]
    public uint GodStatueCounterOnline { get; set; }

    [Column("AverageFieldLevel")]
    public byte AverageFieldLevel { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }
}
