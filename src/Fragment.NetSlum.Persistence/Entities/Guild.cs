using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

public class Guild
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    [MySqlCharSet("sjis")]
    [MySqlCollation("sjis_japanese_ci")]
    public required string Name { get; set; }

    [MySqlCharSet("sjis")]
    [MySqlCollation("sjis_japanese_ci")]
    public string Comment { get; set; } = "";

    public GuildStats Stats { get; set; } = new();

    public byte[] Emblem { get; set; } = Array.Empty<byte>();

    public int? LeaderId { get; set; }
    public Character? Leader { get; set; }

    public ICollection<Character> Members { get; } = new List<Character>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
