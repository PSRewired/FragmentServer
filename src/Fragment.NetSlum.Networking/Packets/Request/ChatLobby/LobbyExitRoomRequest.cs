using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby
{
    [FragmentPacket(MessageType.Data, OpCodes.DataLobbyExitRoom)]
    public class LobbyExitRoomRequest :BaseRequest
    {
        private readonly ILogger<LobbyExitRoomRequest> _logger;
        private readonly ChatLobbyStore _chatLobbyStore;

        public LobbyExitRoomRequest(ILogger<LobbyExitRoomRequest> logger, ChatLobbyStore chatLobbyStore)
        {
            _logger = logger;
            _chatLobbyStore = chatLobbyStore;
        }
        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {
            var chatLobby = _chatLobbyStore.GetLobbyBySession(session);

            if (chatLobby != null)
            {
                var player = chatLobby.GetPlayerByAccountId(session.PlayerAccountId);
                chatLobby.RemovePlayer(player);

            }



            //var chatLobby = _chatLobbyStore.GetLobby()
            BaseResponse response = new LobbyExitResponse();
            return ReturnSingle(response.Build());
        }

    }
}
