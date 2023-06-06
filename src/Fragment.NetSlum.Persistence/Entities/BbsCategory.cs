using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class BbsCategory : IConfigurableEntity<BbsCategory>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    public required string CategoryName { get; set; }

    public ICollection<BbsThread> Threads = new List<BbsThread>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void Configure(EntityTypeBuilder<BbsCategory> entityBuilder)
    {
        entityBuilder.HasData(new BbsCategory
        {
            Id = 1,
            CategoryName = "GENERAL",
            CreatedAt = DateTime.MinValue,
        });
    }
}
