using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(MessageType.Data, OpCodes.DataLobbyEvent)]
public class LobbyEventRequest : BaseRequest
{
    private readonly ILogger<LobbyEventRequest> _logger;
    private readonly ChatLobbyStore _chatLobbyStore;

    public LobbyEventRequest(ILogger<LobbyEventRequest> logger, ChatLobbyStore chatLobbyStore)
    {
        _logger = logger;
        _chatLobbyStore = chatLobbyStore;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var lobbyId = BinaryPrimitives.ReadUInt16BigEndian(request.Data[..2].Span);

        //var chatLobby = _chatLobbyStore.GetLobby(lobbyId);
        var chatLobby = _chatLobbyStore.GetLobbyBySession(session);
        var chatLobbyPlayer = chatLobby?.GetPlayerByCharacterId(session.CharacterId);

        if (chatLobbyPlayer == null)
        {
            throw new DataException($"Could not find lobby or player reference for lobbyId {lobbyId}");
        }

        var response = new LobbyEventResponse()
            .SetData(request.Data[2..])
            .SetSenderIndex(chatLobbyPlayer.PlayerIndex);

        chatLobby?.NotifyAllExcept(chatLobbyPlayer, response.Build());

        var systemMsg = "AAccount activation successful!".ToShiftJis();

        var systemMsgBuffer = new byte[systemMsg.Length + 3];
        systemMsg.CopyTo(systemMsgBuffer.AsSpan()[3..]);

        var response2 = new LobbyEventResponse()
            .SetData(systemMsgBuffer)
            .SetSenderIndex(chatLobbyPlayer.PlayerIndex);

        return ValueTask.FromResult<ICollection<FragmentMessage>>([response.SetIsSender(true).Build(), response2.SetIsSender(true).Build()]);
        return SingleMessage(
            response
                .SetIsSender(true)
                .Build()
        );
    }
}
