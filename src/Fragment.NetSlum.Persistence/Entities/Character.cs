using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Core.Constants;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

public class Character
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int PlayerAccountId { get; set; }
    public required PlayerAccount PlayerAccount { get; set; }

    public int StatsId { get; set; }
    public required CharacterStats Stats { get; set; }

    public int CurrencyId { get; set; }
    public required CharacterCurrency Currency { get; set; }

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
}
