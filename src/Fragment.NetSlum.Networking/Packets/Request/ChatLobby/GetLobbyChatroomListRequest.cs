using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class GetLobbyChatroomListRequest:BaseRequest
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
            //var channels = _database.ChatLobbies.Where(c => c.DefaultChannel == true).ToList();
            var chatLobbies = _chatLobbyStore.ChatLobbies;
            var responses = new List<FragmentMessage>();

            //Add the ChatLobby count response to the collection list
            responses.Add(new ChatLobbyCountResponse().SetChatLobbyCount((ushort)chatLobbies.Count).Build());

            //Build the Chat Lobby List
            responses.AddRange(chatLobbies.Select(c => new ChatLobbyEntryResponse()
                .SetChatLobbyName(c.LobbyName)
                .SetChatLobbyId((ushort)c.LobbyId)
                .SetClientCount(c.PlayerCount)
                .Build()));

            return Task.FromResult<ICollection<FragmentMessage>>(responses);
        }
    }
}
