using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Core.Entities;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username", TypeName = "text")]
    public string Username { get; set; } = null!;

    [Column("email", TypeName = "text")]
    public string Email { get; set; } = null!;

    [Column("password", TypeName = "text")]
    public string Password { get; set; } = null!;

    [Column("enabled")]
    public sbyte Enabled { get; set; }

    [Column("admin")]
    public sbyte Admin { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [Column("email_verified_at", TypeName = "timestamp")]
    public DateTime? EmailVerifiedAt { get; set; }

    [Column("remember_token")]
    [StringLength(100)]
    public string? RememberToken { get; set; }

    [Column("theme")]
    public int Theme { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }
}
