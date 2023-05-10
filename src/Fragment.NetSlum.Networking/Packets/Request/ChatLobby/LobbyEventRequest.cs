using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby
{
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
            var response = new List<LobbyEventResponse>();

            ushort playerIndex = _chatLobbyStore.GetLobby(session.ChatRoomId).GetPlayerByAccountId(session.PlayerAccountId).PlayerIndex;

            //We have to send out a status update to all clients in this chat room but I don't understand where that comes from?
            foreach (var c in session.Server.Sessions)
            {
                var playerSession = ((FragmentTcpSession)c);
                if (playerSession.PlayerAccountId != session.PlayerAccountId && session.ChatRoomId == playerSession.ChatRoomId)
                {
                    c.SendAsync(new LobbyEventResponse()
                        .SetData(request.Data.ToArray())
                        .SetSenderIndex(playerIndex)
                        .SetIsSender(false).Build().ToArray());


                }
                else
                {
                    c.SendAsync(new LobbyEventResponse()
                        .SetData(request.Data.ToArray())
                        .SetSenderIndex(playerIndex)
                        .SetIsSender(true).Build().ToArray());
                }
            }
            return Task.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
        }
    }
}
