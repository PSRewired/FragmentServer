using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Core.Constants;

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
        ChatLobbyType chatType = (ChatLobbyType)(BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[2..4]));
        var chatLobby = _chatLobbyStore.GetLobby(chatLobbyId);

        if(chatType == ChatLobbyType.Guild)
        {
            chatLobbyId++;
            chatLobby = _chatLobbyStore.GetLobby(chatLobbyId, true);
        }

        if (chatLobby == null)
        {
            throw new ArgumentException($"Attempted to enter {chatLobbyId} which is not a valid chat room");
        }

        var myPlayer = new ChatLobbyPlayer(session);
        chatLobby.AddPlayer(myPlayer);

        // We need to send the current lobby state before adding the player to the chat lobby
        var responses = new List<FragmentMessage>
        {
            new ChatLobbyEnterRoomResponse()
                .SetClientCount(chatLobby.PlayerCount)
                .Build()
        };

        foreach (var player in chatLobby.GetPlayers())
        {
            responses.Add(new ChatLobbyStatusUpdateResponse()
                .SetLastStatus(player.TcpSession.LastStatus)
                .Build());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
