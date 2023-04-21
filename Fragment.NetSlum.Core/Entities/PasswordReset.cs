using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Core.Entities;

[Keyless]
[Table("password_resets")]
public class PasswordReset
{
    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("token")]
    [StringLength(255)]
    public string? Token { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }
}
