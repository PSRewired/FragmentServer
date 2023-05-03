using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

public class WebNewsArticle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    public ushort? WebNewsCategoryId { get; set; }
    public WebNewsCategory? WebNewsCategory { get; set; }

    [MySqlCharSet("sjis")]
    [MySqlCollation("sjis_japanese_ci")]
    [MaxLength(33)]
    public required string Title { get; set; }

    [MySqlCharSet("sjis")]
    [MySqlCollation("sjis_japanese_ci")]
    [MaxLength(412)]
    public string Content { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
