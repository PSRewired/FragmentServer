using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

[Index(nameof(PublicIpAddress))]
[Index(nameof(LocalIpAddress))]
public class AreaServerAssociation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public AuthUser AuthUser { get; set; } = default!;
    public Guid AuthUserId { get; set; }

    [MaxLength(15)]
    public required string PublicIpAddress { get; set; }
    [MaxLength(15)]
    public required string LocalIpAddress { get; set; }

    [MaxLength(21)]
    public required string LastKnownName { get; set; }

    [Timestampable(EntityState.Added)]
    public DateTime CreatedAt { get; set; }

    [Timestampable(EntityState.Modified)]
    public DateTime? UpdatedAt { get; set; }
}
