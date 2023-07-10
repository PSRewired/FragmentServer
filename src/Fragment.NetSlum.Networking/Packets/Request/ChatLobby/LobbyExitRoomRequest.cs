using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
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
            var player = chatLobby?.GetPlayerByCharacterId(session.CharacterId);

            if (player != null)
            {
                chatLobby?.RemovePlayer(player);
            }

            if (chatLobby?.PlayerCount < 1 && chatLobby?.LobbyType == ChatLobbyType.Guild)
            {
                _logger.LogInformation("Removing {LobbyName} since the last player in it has left", chatLobby.LobbyName);
                _chatLobbyStore.RemoveChatLobbyById(chatLobby.LobbyId);
            }

            return SingleMessage(new LobbyExitResponse().Build());
        }

    }
}
