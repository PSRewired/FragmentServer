using System;

namespace Fragment.NetSlum.Server.Api.Models;

public record AuthUserDetail(Guid Id, string Username, int? RoleId, DateTime CreatedAt);
