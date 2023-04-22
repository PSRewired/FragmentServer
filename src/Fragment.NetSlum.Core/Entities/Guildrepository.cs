using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Core.Entities;

[Table("guildrepository")]
[MySqlCharSet("sjis")]
[MySqlCollation("sjis_japanese_ci")]
public class Guildrepository
{
    [Key]
    [Column("guildID")]
    public int GuildId { get; set; }

    [Column("guildName", TypeName = "blob")]
    public byte[]? GuildName { get; set; }

    [Column("guildEmblem", TypeName = "blob")]
    public byte[]? GuildEmblem { get; set; }

    [Column("guildComment", TypeName = "blob")]
    public byte[]? GuildComment { get; set; }

    [Column("establishmentDate")]
    [StringLength(255)]
    public string? EstablishmentDate { get; set; }

    [Column("masterPlayerID")]
    public int? MasterPlayerId { get; set; }

    [Column("goldCoin")]
    public int? GoldCoin { get; set; }

    [Column("silverCoin")]
    public int? SilverCoin { get; set; }

    [Column("bronzeCoin")]
    public int? BronzeCoin { get; set; }

    [Column("gp")]
    public int? Gp { get; set; }
}
