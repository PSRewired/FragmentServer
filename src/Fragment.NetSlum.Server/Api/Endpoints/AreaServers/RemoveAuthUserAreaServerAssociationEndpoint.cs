using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.AreaServers;

public record RemoveAuthUserAreaServerAssociationRequest(int AssociationId);

public class RemoveAuthUserAreaServerAssociationEndpoint : Endpoint<RemoveAuthUserAreaServerAssociationRequest, EmptyResponse>
{
    private readonly FragmentContext _database;

    public RemoveAuthUserAreaServerAssociationEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Delete("/users/areaservers/{associationId:int}");
        Summary(s =>
        {
            s.Summary = "Removes user association with the specified area server";
        });
    }

    public override async Task HandleAsync(RemoveAuthUserAreaServerAssociationRequest req, CancellationToken ct)
    {
        var userId = Guid.Parse(HttpContext.User.FindFirst("authUserId")!.Value);

        var server = await _database.AreaServerAssociations
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == req.AssociationId && a.AuthUserId == userId, cancellationToken: ct);

        if (server == null)
        {
            throw new KeyNotFoundException($"Unknown association {req.AssociationId}");
        }

        _database.AreaServerAssociations.Remove(server);
        await _database.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}
