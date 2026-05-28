using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Auth;

public record UpdateAuthUserRequest(Guid UserId, int? RoleId);

public class UpdateAuthUserEndpoint : Endpoint<UpdateAuthUserRequest, AuthUserDetail>
{
    private readonly FragmentContext _database;

    public UpdateAuthUserEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Patch("/users/{userId:guid}");
        Permissions(nameof(AuthUserPermissions.ManageUsers));
        Summary(s => { s.Summary = "Update an auth user"; });
    }

    public override async Task<AuthUserDetail> ExecuteAsync(UpdateAuthUserRequest req, CancellationToken ct)
    {
        var user = await _database.AuthUsers
            .FirstOrDefaultAsync(u => u.Id == req.UserId, cancellationToken: ct);

        if (user == null)
        {
            throw new KeyNotFoundException($"Auth user with ID of {req.UserId} does not exist");
        }

        if (req.RoleId.HasValue)
        {
            HandleRoleIdAssignment(req.RoleId.Value, user);
        }

        await _database.SaveChangesAsync(ct);

        return AuthUserMapper.Map(user);
    }

    private void HandleRoleIdAssignment(int roleId, AuthUser user)
    {
        // Passing 0 via the api explicitly unassigns the role
        if (roleId == 0)
        {
            user.RoleId = null;

            return;
        }

        if (!_database.AuthRoles.Any(r => r.Id == roleId))
        {
            throw new KeyNotFoundException($"Role specified does not exist");
        }

        user.RoleId = roleId;
    }
}
