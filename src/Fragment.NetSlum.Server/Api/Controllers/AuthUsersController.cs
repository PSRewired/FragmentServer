using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Networking.Queries.Authentication;
using Fragment.NetSlum.Server.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class AuthUsersController : ControllerBase
{
    private readonly ICommandBus _commandBus;

    public AuthUsersController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpGet("profile")]
    public async Task<AuthUserProfile> GetProfile()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        var userId = Guid.Parse(identity!.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        return new AuthUserProfile
        {
            Permissions = await _commandBus.GetResult(new GetAuthUserPermissionsQuery(userId), HttpContext.RequestAborted),
        };
    }
}
