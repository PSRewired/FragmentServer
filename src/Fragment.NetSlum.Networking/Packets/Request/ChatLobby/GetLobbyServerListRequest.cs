using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.TcpServer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(MessageType.Data, OpCodes.DataLobbyGetServersGetList)]
public class GetLobbyServerListRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetLobbyServerListRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var categoryId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        if (categoryId == 0)
        {
            return ValueTask.FromResult(HandleCategories());
        }

        var category = _database.AreaServerCategories
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == categoryId);

        var areaServers = session.Server.Sessions
            .Cast<FragmentTcpSession>()
            .Where(s => s.IsAreaServer)
            .ToArray();

        var responses = new List<FragmentMessage>();

        responses.Add(new LobbyServerEntryCountResponse((ushort)areaServers.Length).Build());

        ushort cId = 0;
        foreach (var server in areaServers)
        {
            // If the game-client IP matches the recorded private IP address of the area-server, we need to send back their local IP
            // so they are able to connect without NAT
            var clientIpMatchesPrivate = server.AreaServerInfo!.PrivateConnectionEndpoint != null &&
                                                 server.AreaServerInfo!.PrivateConnectionEndpoint.Address.Equals(
                                            IPAddress.Parse(session.Socket!.GetClientIp()));

            responses.Add(new LobbyServerEntryResponse()
                .SetServerId(cId++)
                .SetLevel(server.AreaServerInfo!.Level)
                .SetStatus(server.AreaServerInfo.State)
                .SetExternalAddress((clientIpMatchesPrivate ? server.AreaServerInfo!.PrivateConnectionEndpoint : server.AreaServerInfo!.PublicConnectionEndpoint)!)
                .SetDetails(server.AreaServerInfo.Detail)
                .SetPlayerCount(server.AreaServerInfo.CurrentPlayerCount)
                .SetServerName(server.AreaServerInfo.ServerName)
                .Build());
        }

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }

    private ICollection<FragmentMessage> HandleCategories()
    {
        var responses = new List<FragmentMessage>();

        var categories = _database.AreaServerCategories.ToArray();

        responses.Add(new LobbyServerCategoryCountResponse((ushort)categories.Length).Build());

        foreach (var category in categories)
        {
            responses.Add(new LobbyServerCategoryEntryResponse()
                .SetCategoryId(category.Id)
                .SetCategoryName(category.CategoryName)
                .Build());
        }

        return responses;
    }
}
