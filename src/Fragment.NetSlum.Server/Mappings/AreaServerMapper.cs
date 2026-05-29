using System.Linq;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class AreaServerMapper
{
    [MapProperty(nameof(AreaServerInformation.ServerName), nameof(AreaServerStatus.Name))]
    [MapProperty(nameof(AreaServerInformation.ActiveSince), nameof(AreaServerStatus.OnlineSince))]
    [MapperIgnoreSource(nameof(AreaServerInformation.DiskId))]
    [MapperIgnoreSource(nameof(AreaServerInformation.PublicConnectionEndpoint))]
    [MapperIgnoreSource(nameof(AreaServerInformation.PrivateConnectionEndpoint))]
    [MapperIgnoreSource(nameof(AreaServerInformation.ServerId))]
    public static partial AreaServerStatus Map(AreaServerInformation input);

    public static partial IQueryable<AssociatedAreaServer> ToAssociatedAreaServers(this IQueryable<AreaServerAssociation> query);
    public static partial AssociatedAreaServer Map(AreaServerAssociation server);
}
