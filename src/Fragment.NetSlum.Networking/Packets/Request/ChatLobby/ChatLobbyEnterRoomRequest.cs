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
            ushort chatLobbyId = (ushort)(BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[0..2]) -1);

            var chatLobby = _chatLobbyStore.GetLobby(chatLobbyId);

            chatLobby.AddPlayer(new Models.ChatLobbyPlayerModel() { PlayerAccountId = session.PlayerAccountId });

            session.ChatRoomId = chatLobbyId;
            var clientCount = session.Server.Sessions.Where(c => ((FragmentTcpSession)c).ChatRoomId == chatLobbyId).Count();
            var response = new ChatLobbyEnterRoomResponse().SetClientCount((ushort)clientCount).Build();

            //We have to send out a status update to all clients in this chat room but I don't understand where that comes from?
            foreach( var c in session.Server.Sessions )
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
