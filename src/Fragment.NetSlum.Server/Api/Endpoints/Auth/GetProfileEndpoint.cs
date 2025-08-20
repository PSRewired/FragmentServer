using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Networking.Queries.Authentication;
using Fragment.NetSlum.Server.Api.Models;

namespace Fragment.NetSlum.Server.Api.Endpoints.Auth;

public class GetProfileEndpoint : Endpoint<EmptyRequest, AuthUserProfile>
{
    private readonly ICommandBus _commandBus;

    public GetProfileEndpoint(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public override void Configure()
    {
        Get("/users/profile");
        Roles("User");
    }

    public override async Task<AuthUserProfile> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = Guid.Parse(identity!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        return new AuthUserProfile
        {
            Permissions = await _commandBus.GetResult(new GetAuthUserPermissionsQuery(userId), ct),
        };
    }
}
