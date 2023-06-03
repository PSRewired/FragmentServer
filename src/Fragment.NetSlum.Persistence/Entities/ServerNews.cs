using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class ServerNews : IConfigurableEntity<ServerNews>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(500)]
    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void Configure(EntityTypeBuilder<ServerNews> entityBuilder)
    {
        var motd = @"Welcome to Netslum-Redux!
Current Status:
- Lobby #GOnline#W!
- BBS #GOnline#W!
- Mail #GOnline#W!
- Guilds #GOnline#W!
- Ranking #GOnline#W!
- News #GOnline#W!";

        entityBuilder.HasData(new ServerNews
        {
            Id = 1,
            Content = motd,
            CreatedAt = DateTime.MinValue,
        });
    }
}
