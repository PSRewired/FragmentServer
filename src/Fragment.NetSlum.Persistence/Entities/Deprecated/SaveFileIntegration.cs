using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Keyless]
[Table("save_file_integrations")]
public class SaveFileIntegration
{
    [Column("id")]
    public ulong? Id { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [Column("account")]
    [StringLength(255)]
    public string? Account { get; set; }

    [Column("saveid")]
    [StringLength(255)]
    public string? Saveid { get; set; }

    [Column("token")]
    [StringLength(255)]
    public string? Token { get; set; }

    [Column("tokenExpiry")]
    [StringLength(255)]
    public string? TokenExpiry { get; set; }
}
