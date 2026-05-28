using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Stores;

namespace Fragment.NetSlum.Server.Api.Endpoints.AreaServers;

public record GetAreaServerAssociationClaimStatusRequest(string Code);

public class GetAreaServerAssociationClaimStatusEndpoint : Endpoint<GetAreaServerAssociationClaimStatusRequest, AreaServerClaimDetail>
{
    private readonly AreaServerAssociationStore _areaServerAssociationStore;

    public GetAreaServerAssociationClaimStatusEndpoint(AreaServerAssociationStore areaServerAssociationStore)
    {
        _areaServerAssociationStore = areaServerAssociationStore;
    }

    public override void Configure()
    {
        Get("/areaservers/associations");
        Summary(s =>
        {
            s.Summary = "Gets the current status of a area server claim code";
        });
    }

    public override Task<AreaServerClaimDetail> ExecuteAsync(GetAreaServerAssociationClaimStatusRequest req, CancellationToken ct)
    {
        var claimStatus = _areaServerAssociationStore.GetClaimStatus(req.Code);

        // This intentionally always returns whatever code the user sends to stop API fuzzing
        return Task.FromResult(new AreaServerClaimDetail(req.Code, claimStatus));
    }
}
