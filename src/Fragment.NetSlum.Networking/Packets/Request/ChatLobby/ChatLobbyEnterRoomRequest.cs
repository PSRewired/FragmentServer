using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Models;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(OpCodes.Data, OpCodes.DataLobbyEnterRoomRequest)]
public class ChatLobbyEnterRoomRequest:BaseRequest
{
    private readonly ILogger<ChatLobbyEnterRoomRequest> _logger;
    private readonly ChatLobbyStore _chatLobbyStore;
    public ChatLobbyEnterRoomRequest(ILogger<ChatLobbyEnterRoomRequest> logger, ChatLobbyStore chatLobbyStore)
    {
        _logger = logger;
        _chatLobbyStore = chatLobbyStore;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort chatLobbyId = (ushort)(BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]) -1);

        var chatLobby = _chatLobbyStore.GetLobby(chatLobbyId);

        var myPlayer = new ChatLobbyPlayer(session);
        chatLobby.AddPlayer(myPlayer);

        session.ChatRoomId = chatLobbyId;
        var clientCount = session.Server.Sessions.Count(c => ((FragmentTcpSession)c).ChatRoomId == chatLobbyId);

        var response = new ChatLobbyEnterRoomResponse()
            .SetClientCount((ushort)clientCount)
            .Build();

        //We have to send out a status update to all clients in this chat room but I don't understand where that comes from?
        chatLobby.NotifyAllExcept(myPlayer, new ChatLobbyStatusUpdateResponse()
            .SetLastStatus(session.LastStatus)
            .Build());

        return Task.FromResult<ICollection<FragmentMessage>>(new[] { response });
    }
}
