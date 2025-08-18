using System;

namespace Fragment.NetSlum.Core.Constants;

[Flags]
public enum AuthUserPermissions : int
{
    None = 0,

    ManageNews = 1 << 0,
    ManageUsers = 1 << 1,
}
