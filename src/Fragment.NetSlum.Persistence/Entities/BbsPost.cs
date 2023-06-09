using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

public class BbsPost : IConfigurableEntity<BbsPost>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ThreadId { get; set; }
    public BbsThread Thread { get; set; } = default!;

    public int PostedById { get; set; }
    public Character PostedBy { get; set; } = default!;

    public required string Title { get; set; }

    public BbsPostContent? PostContent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void Configure(EntityTypeBuilder<BbsPost> entityBuilder)
    {
        entityBuilder
            .HasOne(p => p.PostContent)
            .WithOne(c => c.Post)
            .HasForeignKey<BbsPostContent>(c => c.PostId)
            .IsRequired();
    }
}
