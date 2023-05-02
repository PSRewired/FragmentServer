using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

[Index(nameof(SaveId), IsUnique = true)]
public class PlayerAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(20)]
    public required string SaveId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
