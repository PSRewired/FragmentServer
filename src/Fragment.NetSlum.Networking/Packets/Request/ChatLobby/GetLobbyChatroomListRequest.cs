using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby
{
    [FragmentPacket(MessageType.Data, OpCodes.DataGetLobbyChatroomListRequest)]
    public class GetLobbyChatroomListRequest : BaseRequest
    {
        private readonly ILogger<GetLobbyChatroomListRequest> _logger;
        private readonly ChatLobbyStore _chatLobbyStore;

        public GetLobbyChatroomListRequest(ILogger<GetLobbyChatroomListRequest> logger, ChatLobbyStore chatLobbyStore)
        {
            _logger = logger;
            _chatLobbyStore = chatLobbyStore;
        }

        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {
            var availableLobbies = _chatLobbyStore.GetLobbiesByType(ChatLobbyType.Player);

            if (availableLobbies.Length < 1)
            {
                return ReturnSingle(new LobbyChatroomCategoryCountResponse(2).Build());
            }

            var responses = new List<FragmentMessage>();

            foreach (var lobby in availableLobbies)
            {
                responses.Add(
                    new LobbyChatroomCategoryEntryResponse()
                        .SetCategoryId(lobby.LobbyId)
                        .SetCategoryName(lobby.LobbyName)
                        .SetCurrentPlayerCount(lobby.PlayerCount)
                        .Build());
            }

            return Task.FromResult<ICollection<FragmentMessage>>(responses);
        }
    }
}
