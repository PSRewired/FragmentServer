using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby
{
    [FragmentPacket(OpCodes.Data, OpCodes.DataLobbyStatusUpdate)]
    public class ChatLobbyStatusUpdateRequest:BaseRequest
    {
        private readonly ILogger<ChatLobbyStatusUpdateRequest> _logger;
        private readonly ChatLobbyStore _chatLobbyStore;
        public ChatLobbyStatusUpdateRequest(ILogger<ChatLobbyStatusUpdateRequest> logger, ChatLobbyStore chatLobbyStore)
        {
            _logger = logger;
            _chatLobbyStore = chatLobbyStore;
        }

        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {

            var buffer = new Memory<byte>(new byte[request.Length + 2]);
            var bufferSpan = buffer.Span;
            var cl = _chatLobbyStore.GetLobby(session.ChatRoomId);
            ushort playerIndex = cl.GetPlayerByAccountId(session.PlayerAccountId).PlayerIndex;
            ushort clientCount = cl.PlayerCount;
            var response = new ChatLobbyEnterRoomResponse().SetClientCount((ushort)clientCount).Build();

            //We have to send out a status update to all clients in this chat room but I don't understand where that comes from?
            foreach (var c in session.Server.Sessions)
            {
                var playerSession = ((FragmentTcpSession)c);
                if (playerSession.PlayerAccountId != session.PlayerAccountId && session.ChatRoomId == playerSession.ChatRoomId)
                {

                    c.SendAsync(new ChatLobbyStatusUpdateResponse()
                        .SetLastStatus(session.LastStatus)
                        .Build().ToArray());
                }
            }
            return Task.FromResult<ICollection<FragmentMessage>>(new[] { response });
        }
    }
}
