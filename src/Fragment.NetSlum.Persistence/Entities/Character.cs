using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class Character : IConfigurableEntity<Character>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int PlayerAccountId { get; set; }
    public PlayerAccount? PlayerAccount { get; set; }

    public CharacterStats CharacterStats { get; set; } = new();
    public CharacterCurrency CharacterCurrency { get; set; } = new();

    [MySqlCharSet("sjis")]
    [MySqlCollation("sjis_japanese_ci")]
    public required string CharacterName { get; set; }

    [MySqlCharSet("sjis")]
    [MySqlCollation("sjis_japanese_ci")]
    public required string GreetingMessage { get; set; }

    public byte SaveSlotId { get; set; }

    public required int CurrentLevel { get; set; }

    public required uint FullModelId { get; set; }
    public required CharacterClass Class { get; set; }

    void IConfigurableEntity<Character>.Configure(EntityTypeBuilder<Character> entityBuilder)
    {
    }
}