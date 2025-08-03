using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Utils;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.TcpServer.Extensions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.DataLobbyFavoritesAsInquiry)]
public class AreaServerFavoritesRequest : BaseRequest
{
    private readonly ILogger<AreaServerFavoritesRequest> _logger;

    public AreaServerFavoritesRequest(ILogger<AreaServerFavoritesRequest> logger)
    {
        _logger = logger;
    }
    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var areaServers = session.Server.Sessions
            .Cast<FragmentTcpSession>()
            .Where(s => s.IsAreaServer)
            .ToArray();

        var responses = new List<FragmentMessage>();


        foreach (var areaServer in areaServers)
        {
            var clientIpMatchesPrivate = areaServer.AreaServerInfo!.PrivateConnectionEndpoint != null &&
                                         areaServer.AreaServerInfo!.PrivateConnectionEndpoint.Address.Equals(
                                             IPAddress.Parse(session.Socket!.GetClientIp()));

            var resp = new AreaServerFavoriteEntry()
                .SetLevel(areaServer.AreaServerInfo!.Level)
                .SetStatus(areaServer.AreaServerInfo.State)
                .SetExternalAddress((clientIpMatchesPrivate ? areaServer.AreaServerInfo!.PrivateConnectionEndpoint : areaServer.AreaServerInfo!.PublicConnectionEndpoint)!)
                .SetDetails(areaServer.AreaServerInfo.Detail)
                .SetPlayerCount(areaServer.AreaServerInfo.CurrentPlayerCount)
                .SetServerName(ServerNameUtil.FormatServerName(areaServer.AreaServerInfo.ServerName))
                .Build();

            responses.Add(resp);

            _logger.LogDebug("Area Server: {AreaServer}", areaServer.AreaServerInfo.ToString());
        }

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
