using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(OpCodes.Data, OpCodes.DataLobbyGetServersGetList)]
public class GetLobbyServerListRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetLobbyServerListRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var categoryId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        if (categoryId == 0)
        {
            return Task.FromResult(HandleCategories());
        }

        var areaServers = session.Server.Sessions
            .Cast<FragmentTcpSession>()
            .Where(s => s.IsAreaServer);

        return Task.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
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
