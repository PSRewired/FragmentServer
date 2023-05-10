using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby
{
    [FragmentPacket(OpCodes.Data, OpCodes.DataLobbyGetMenuRequest)]
    public class ChatLobbyGetMenuRequest:BaseRequest
    {
        private readonly ILogger<ChatLobbyGetMenuRequest> _logger;
        private readonly ChatLobbyStore _chatLobbyStore;

        public ChatLobbyGetMenuRequest(ILogger<ChatLobbyGetMenuRequest> logger, ChatLobbyStore chatLobbyStore)
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

            var sessions = session.Server.Sessions;
            //Build the Chat Lobby List
            responses.AddRange(chatLobbies.Select(c => new ChatLobbyEntryResponse()
                .SetChatLobbyName(c.ChatLobby.ChatLobbyName)
                .SetChatLobbyId((ushort)c.ChatLobby.Id)
                .SetClientCount((ushort)sessions.Where(s => ((FragmentTcpSession)s).ChatRoomId == c.ChatLobby.Id).Count())
                .Build()));

            return Task.FromResult<ICollection<FragmentMessage>>(responses);
        }

    }
}
