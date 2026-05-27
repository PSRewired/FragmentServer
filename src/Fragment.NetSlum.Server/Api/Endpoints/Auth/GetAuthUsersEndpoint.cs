using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.Auth;

public record GetAuthUsersRequest(string? Name = null, int Page = 1, int PageSize = 50);

public class GetAuthUsersEndpoint : Endpoint<GetAuthUsersRequest, PagedResult<AuthUserDetail>>
{
    private readonly FragmentContext _database;

    public GetAuthUsersEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/users");
        Permissions(nameof(AuthUserPermissions.ManageUsers));
        Summary(s => { s.Summary = "Look up registered user information"; });
    }

    public override Task<PagedResult<AuthUserDetail>> ExecuteAsync(GetAuthUsersRequest req, CancellationToken ct)
    {
        var userQuery = _database.AuthUsers
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(req.Name))
        {
            userQuery = userQuery.Where(u => u.Username.Contains(req.Name));
        }

        var transformedQuery = userQuery
            .OrderBy(u => u.Username)
            .ToAuthUserDetail();

        var userCount = transformedQuery.Count();
        var users = transformedQuery.Paginate(req.Page, req.PageSize);

        return Task.FromResult(new PagedResult<AuthUserDetail>(req.Page, req.PageSize, userCount, users.ToList()));
    }
}
