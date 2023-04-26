using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Keyless]
[Table("migrations")]
public class Migration
{
    [Column("id")]
    public uint? Id { get; set; }

    [Column("migration")]
    [StringLength(255)]
    public string? Migration1 { get; set; }

    [Column("batch")]
    public int? Batch { get; set; }
}
