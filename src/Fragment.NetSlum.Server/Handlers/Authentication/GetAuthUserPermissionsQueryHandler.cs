using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Queries.Authentication;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Handlers.Authentication;

public class GetAuthUserPermissionsQueryHandler : QueryHandler<GetAuthUserPermissionsQuery, IEnumerable<string>>
{
    private readonly FragmentContext _database;

    /// <summary>
    /// Names of roles that should be excluded
    /// </summary>
    private static readonly string[] RoleExclusions =
    [
        nameof(AuthUserPermissions.None),
    ];

    public GetAuthUserPermissionsQueryHandler(FragmentContext database)
    {
        _database = database;
    }

    public override async ValueTask<IEnumerable<string>> Handle(GetAuthUserPermissionsQuery command, CancellationToken cancellationToken)
    {
        var user =  await _database.AuthUsers
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == command.Username, cancellationToken);

        return user?.Role == null
            ? []
            : user.Role.PermissionMask.ToStringCollection().Where(r => !RoleExclusions.Contains(r));
    }
}
