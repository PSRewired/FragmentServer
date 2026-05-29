using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Endpoints.AreaServers;

public class GetAuthUserAreaServerAssociationsEndpoint : Endpoint<EmptyRequest, IEnumerable<AssociatedAreaServer>>
{
    private readonly FragmentContext _database;

    public GetAuthUserAreaServerAssociationsEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Get("/users/areaservers");
        Summary(s =>
        {
            s.Summary = "Returns all area servers associated with the authenticated account";
        });
    }

    public override Task<IEnumerable<AssociatedAreaServer>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var userId = Guid.Parse(HttpContext.User.FindFirst("authUserId")!.Value);

        var servers = _database.AreaServerAssociations
            .AsNoTracking()
            .Where(a => a.AuthUserId == userId)
            .ToAssociatedAreaServers()
            .AsEnumerable();

        return Task.FromResult(servers);
    }
}
