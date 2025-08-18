using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fragment.NetSlum.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(EmailAddress))]
public class AuthUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(64)]
    public required string Username { get; set; }

    [NotMapped]
    private string _emailAddress = default!;

    [MaxLength(320)] //RFC-5321
    public required string EmailAddress { get => _emailAddress.ToLowerInvariant() ; set => _emailAddress = value; }

    public int? RoleId { get; set; }
    public AuthRole? Role { get; set; }

    [Timestampable(EntityState.Added)]
    public DateTime CreatedAt { get; set; }
}
