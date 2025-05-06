using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.TcpServer.Extensions;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.DataLobbyFavoritesAsInquiry)]
public class AreaServerFavoritesRequest : BaseRequest
{
    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var areaServers = session.Server.Sessions
            .Cast<FragmentTcpSession>()
            .Where(s => s.IsAreaServer)
            .ToArray();

        var responses = new List<FragmentMessage>();

        responses.Add(new AreaServerFavoritesCountResponse((ushort)areaServers.Length).Build());

        foreach (var areaServer in areaServers)
        {
            var clientIpMatchesPrivate = areaServer.AreaServerInfo!.PrivateConnectionEndpoint != null &&
                                         areaServer.AreaServerInfo!.PrivateConnectionEndpoint.Address.Equals(
                                             IPAddress.Parse(session.Socket!.GetClientIp()));
            responses.Add(new AreaServerFavoriteEntry()
                .SetUserNum((ushort)areaServer.PlayerAccountId)
                .SetLocation((clientIpMatchesPrivate ? areaServer.AreaServerInfo!.PrivateConnectionEndpoint : areaServer.AreaServerInfo!.PublicConnectionEndpoint)!)
                .SetDetail(areaServer.AreaServerInfo.Detail)
                .Build());
        }

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
