using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class Guild : IConfigurableEntity<Guild>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    public required string Name { get; set; }

    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    public string Comment { get; set; } = "";

    public GuildStats Stats { get; set; } = new();

    public byte[] Emblem { get; set; } = [];

    public int? LeaderId { get; set; }
    public Character? Leader { get; set; }

    public ICollection<Character> Members { get; } = new List<Character>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void Configure(EntityTypeBuilder<Guild> entityBuilder)
    {
        entityBuilder
            .HasMany(e => e.Members)
            .WithOne(e => e.Guild)
            .HasForeignKey(e => e.GuildId);

        entityBuilder
            .HasOne(e => e.Leader)
            .WithOne()
            .HasForeignKey<Guild>(e => e.LeaderId);
    }
}
