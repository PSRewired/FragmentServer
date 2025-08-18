using System.Collections.Generic;

namespace Fragment.NetSlum.Server.Api.Models;

public class AuthUserProfile
{
    public IEnumerable<string> Permissions { get; set; } = [];
}
