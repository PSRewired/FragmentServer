using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace Fragment.NetSlum.Server.Transformers;

public class WebUserClaimsTransformer : IClaimsTransformation
{
    private readonly IFusionCache _cache;
    private readonly FragmentContext _database;

    public WebUserClaimsTransformer(IFusionCache cache, FragmentContext database)
    {
        _cache = cache;
        _database = database;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ({ IsAuthenticated: true } and ClaimsIdentity claimsIdentity))
        {
            return principal;
        }

        var user = await GetAuthUserByUsername(claimsIdentity);

        if (user?.Role != null)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()!));

            foreach (var permName in user.Role.PermissionMask.ToStringCollection())
            {
                claimsIdentity.AddClaim(new Claim("permissions", permName));
            }
        }

        return principal;
    }

    private async Task<AuthUser?> GetAuthUserByUsername(ClaimsIdentity identity)
    {
        var username = identity.FindFirst(c => c.Type == "username")?.Value;
        var email = identity.FindFirst(ClaimTypes.Email)?.Value;

        // If we have the sub from the token, the web-app maps the sub from the user's discord account. So let's try and capture that here.
        var sub = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;


        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        AuthUser? user = await _cache.GetOrDefaultAsync<AuthUser>($"auth_user:{username}");

        if (user != null)
        {
            return user;
        }

        user = await _database.AuthUsers
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username) ?? await CreateAuthUser(sub, username, email);

        await _cache.SetAsync($"auth_user:{username}", user);

        return user;
    }

    private async Task<AuthUser> CreateAuthUser(string? sub, string username, string email)
    {
        if (!Guid.TryParse(sub, out var subGuid))
        {
            subGuid = Guid.NewGuid();
        }

        var user =  new AuthUser
        {
            Id = subGuid,
            Username = username,
            EmailAddress = email,
        };

        try
        {
            _database.AuthUsers.Add(user);
            await _database.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // If multiple web-requests come in, it's entirely possible for a race condition to occur so we need to handle that.
            _database.ChangeTracker.Clear();

            return await _database.AuthUsers
                .Include(u => u.Role)
                .FirstAsync(u => u.Username == username);
        }

        return user;
    }
}
