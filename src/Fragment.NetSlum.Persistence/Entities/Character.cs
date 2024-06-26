using System;
using System.Collections.Generic;
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
    public ICollection<CharacterIpLog> IpLogs { get; set; } = new List<CharacterIpLog>();

    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    public required string CharacterName { get; set; }

    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    public required string GreetingMessage { get; set; }

    [MaxLength(32)]
    public string SaveId { get; set; } = default!;
    public byte SaveSlotId { get; set; }

    public required int CurrentLevel { get; set; }

    public required uint FullModelId { get; set; }
    public required CharacterClass Class { get; set; }

    public ushort? GuildId { get; set; }
    public Guild? Guild { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    void IConfigurableEntity<Character>.Configure(EntityTypeBuilder<Character> entityBuilder)
    {
    }
}
