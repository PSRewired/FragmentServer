using System.Linq;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class AuthUserMapper
{
    public static partial IQueryable<AuthUserDetail> ToAuthUserDetail(this IQueryable<AuthUser> source);
    public static partial AuthUserDetail Map(AuthUser user);
}
