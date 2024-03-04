using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public partial class AreaServerMapper
{
    [MapProperty(nameof(AreaServerInformation.ServerName), nameof(AreaServerStatus.Name))]
    [MapProperty(nameof(AreaServerInformation.ActiveSince), nameof(AreaServerStatus.OnlineSince))]
    public static partial AreaServerStatus Map(AreaServerInformation input);
}
