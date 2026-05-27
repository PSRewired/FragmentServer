using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class AuthRoleMapping
{
    [MapProperty(nameof(AuthRole.PermissionMask), nameof(AuthRoleDetail.Permissions))]
    public static partial AuthRoleDetail Map(AuthRole role);
}
