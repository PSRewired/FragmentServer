using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("characterrepository")]
[MySqlCharSet("sjis")]
[MySqlCollation("sjis_japanese_ci")]
public class Characterrepository
{
    [Key]
    [Column("playerID")]
    public int PlayerId { get; set; }

    [Column("characterName", TypeName = "blob")]
    public byte[]? CharacterName { get; set; }

    [Column("classID")]
    public int? ClassId { get; set; }

    [Column("characterLevel")]
    public int? CharacterLevel { get; set; }

    [Column("greeting", TypeName = "blob")]
    public byte[]? Greeting { get; set; }

    [Column("guildID")]
    public int? GuildId { get; set; }

    [Column("guildMaster")]
    public int? GuildMaster { get; set; }

    [Column("modelNumber")]
    public int? ModelNumber { get; set; }

    [Column("onlineStatus")]
    public bool? OnlineStatus { get; set; }

    [Column("accountID")]
    public int? AccountId { get; set; }

    [Column("characterSaveID")]
    [StringLength(255)]
    public string? CharacterSaveId { get; set; }

    [Column("charHP")]
    public int? CharHp { get; set; }

    [Column("charSP")]
    public int? CharSp { get; set; }

    [Column("charGP")]
    public int? CharGp { get; set; }

    [Column("charOnlineGoat")]
    public int? CharOnlineGoat { get; set; }

    [Column("charOfflineGoat")]
    public int? CharOfflineGoat { get; set; }

    [Column("charGoldCoin")]
    public int? CharGoldCoin { get; set; }

    [Column("charSilverCoin")]
    public int? CharSilverCoin { get; set; }

    [Column("charBronzeCoin")]
    public int? CharBronzeCoin { get; set; }
}
