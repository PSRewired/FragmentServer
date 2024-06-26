﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("player_account_id")]
[Index("Saveid", Name = "PLAYER_ACCOUNT_ID_SAVEID_uindex", IsUnique = true)]
[MySqlCharSet("cp932")]
[MySqlCollation("cp932_japanese_ci")]
public class PlayerAccountId
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SAVEID")]
    [StringLength(100)]
    [MySqlCharSet("utf8mb4")]
    [MySqlCollation("utf8mb4_0900_ai_ci")]
    public string Saveid { get; set; } = null!;
}
