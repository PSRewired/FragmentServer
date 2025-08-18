using System;
using System.ComponentModel.DataAnnotations;
using Fragment.NetSlum.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

[Index(nameof(RoleName), IsUnique = true)]
public class AuthRole : IConfigurableEntity<AuthRole>
{
    public int Id { get; set; }

    [MaxLength(256)]
    public required string RoleName { get; set; }

    public AuthUserPermissions PermissionMask { get; set; } = AuthUserPermissions.None;

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedById { get; set; }
    public AuthUser? CreatedBy { get; set; }

    public void Configure(EntityTypeBuilder<AuthRole> entityBuilder)
    {
        entityBuilder
            .HasOne(e => e.CreatedBy)
            .WithOne()
            .HasForeignKey<AuthRole>(e => e.CreatedById)
            .OnDelete(DeleteBehavior.SetNull);

        entityBuilder.HasData(
            new AuthRole { Id = 1, RoleName = "Administrator", PermissionMask = (AuthUserPermissions)int.MaxValue},
            new AuthRole { Id = 2, RoleName = "Moderator", PermissionMask = AuthUserPermissions.ManageUsers },
            new AuthRole { Id = 3, RoleName = "News Editor", PermissionMask = AuthUserPermissions.ManageNews }
        );
    }
}
