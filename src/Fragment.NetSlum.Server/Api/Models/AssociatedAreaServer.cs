using System;

namespace Fragment.NetSlum.Server.Api.Models;

public record AssociatedAreaServer(
    int Id,
    string PublicIpAddress,
    string LocalIpAddress,
    string LastKnownName,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
