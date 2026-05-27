using Fragment.NetSlum.Core.Constants;

namespace Fragment.NetSlum.Server.Api.Models;

public record AuthRoleDetail(int Id, string RoleName, AuthUserPermissions Permissions);
