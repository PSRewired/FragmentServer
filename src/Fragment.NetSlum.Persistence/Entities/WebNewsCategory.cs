using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class WebNewsCategory : IConfigurableEntity<WebNewsCategory>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    [MySqlCharSet("cp932")]
    [MySqlCollation("cp932_japanese_ci")]
    [MaxLength(64)]
    public required string CategoryName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void Configure(EntityTypeBuilder<WebNewsCategory> entityBuilder)
    {
        entityBuilder.HasData(new WebNewsCategory
        {
            Id = 1,
            CategoryName = "Netslum News",
            CreatedAt = DateTime.MinValue,
        });
    }
}
