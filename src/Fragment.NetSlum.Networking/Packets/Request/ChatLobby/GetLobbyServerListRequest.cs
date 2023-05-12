using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(OpCodes.Data, OpCodes.DataLobbyGetServersGetList)]
public class GetLobbyServerListRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        if (request.Data.Span[1] == 0)
        {
            return Task.FromResult(HandleCategories());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
    }

    private ICollection<FragmentMessage> HandleCategories()
    {
        var responses = new List<FragmentMessage>();
        responses.Add(new LobbyServerCategoryCountResponse((ushort)3).Build());
        responses.Add(new LobbyServerCategoryEntryResponse()
            .SetCategoryName("MAIN")
            .Build());
        responses.Add(new LobbyServerCategoryEntryResponse()
            .SetCategoryName("Foobar")
            .Build());
        responses.Add(new LobbyServerCategoryEntryResponse()
            .SetCategoryName("Fizzbuzz")
            .Build());


        return responses;
    }
}
