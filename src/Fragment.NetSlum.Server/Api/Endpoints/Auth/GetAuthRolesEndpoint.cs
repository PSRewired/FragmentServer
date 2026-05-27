using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Auth;

public class GetAuthRolesEndpoint : Endpoint<EmptyRequest, IEnumerable<AuthRoleDetail>>
{
    private readonly FragmentContext _database;

    public GetAuthRolesEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/users/roles");
        Permissions(nameof(AuthUserPermissions.ManageUsers));
        Summary(s => { s.Summary = "Get configured roles and their associated permissions"; });
    }

    public override async Task<IEnumerable<AuthRoleDetail>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await _database.AuthRoles
            .AsNoTracking()
            .OrderBy(x => x.RoleName)
            .Select(r => AuthRoleMapping.Map(r))
            .ToListAsync(cancellationToken: ct);
    }
}
