using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(OpCodes.Data, OpCodes.DataLobbyEvent)]
public class LobbyEventRequest : BaseRequest
{
    private readonly ILogger<LobbyEventRequest> _logger;
    private readonly ChatLobbyStore _chatLobbyStore;

    public LobbyEventRequest(ILogger<LobbyEventRequest> logger, ChatLobbyStore chatLobbyStore)
    {
        _logger = logger;
        _chatLobbyStore = chatLobbyStore;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var chatLobbyPlayer = _chatLobbyStore.GetLobbyBySession(session)?.GetPlayerByAccountId(session.PlayerAccountId);

        if (chatLobbyPlayer == null)
        {
            throw new DataException("Could not find player reference for this session");
        }

        var response = new LobbyEventResponse()
            .SetData(request.Data)
            .SetSenderIndex(chatLobbyPlayer.PlayerIndex);

        //We have to send out a status update to all clients in this chat room but I don't understand where that comes from?
        chatLobbyPlayer.ChatLobby.NotifyAllExcept(chatLobbyPlayer, response.Build());

        return Task.FromResult<ICollection<FragmentMessage>>(new[] { response
            .SetIsSender(true)
            .Build() });
    }
}
