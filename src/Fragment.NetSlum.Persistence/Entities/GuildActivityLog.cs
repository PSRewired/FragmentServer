using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class GuildActivityLog : IConfigurableEntity<GuildActivityLog>
{
    public enum GuildPlayerAction : byte
    {
        PlayerJoined,
        PlayerLeft,
        PlayerKicked,
        LeaderChanged,
        DetailsUpdated,
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public required GuildPlayerAction ActionPerformed { get; set; }

    public ushort GuildId { get; set; }
    public Guild Guild { get; set; } = null!;

    public int? PerformedByCharacterId { get; set; }
    public Character? PerformedByCharacter { get; set; } = null!;

    public int? PerformedOnCharacterId { get; set; }
    public Character? PerformedOnCharacter { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime ActionedOn { get; set; } = DateTime.UtcNow;

    public void Configure(EntityTypeBuilder<GuildActivityLog> entityBuilder)
    {
        entityBuilder
            .Property(e => e.ActionPerformed)
            .HasConversion(
                v => v.ToString(),
                v => (GuildPlayerAction)Enum.Parse(typeof(GuildPlayerAction), v));
    }
}
