using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Stores;
using Mediator;

namespace Fragment.NetSlum.Server.Api.Endpoints.AreaServers;

public class CreateAreaServerAssociationClaimEndpoint : Endpoint<EmptyRequest, AreaServerClaimDetail>
{
    private readonly AreaServerAssociationStore _areaServerAssociationStore;
    private readonly IMediator _mediator;

    public CreateAreaServerAssociationClaimEndpoint(AreaServerAssociationStore areaServerAssociationStore, IMediator mediator)
    {
        _areaServerAssociationStore = areaServerAssociationStore;
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/areaservers/associations");
        Summary(s =>
        {
            s.Summary = "Creates a claim code for area server association";
        });
    }

    public override Task<AreaServerClaimDetail> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var claimCode = _areaServerAssociationStore.GenerateCode(Guid.Parse(HttpContext.User.FindFirst("authUserId")!.Value));

        return Task.FromResult(new AreaServerClaimDetail(claimCode, false));
    }
}
