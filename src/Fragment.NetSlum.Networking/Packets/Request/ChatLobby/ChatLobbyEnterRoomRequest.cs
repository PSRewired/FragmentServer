using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby
{
    [FragmentPacket(OpCodes.Data, OpCodes.DataLobbyEnterRoomRequest)]
    public class ChatLobbyEnterRoomRequest:BaseRequest
    {
        private readonly ILogger<ChatLobbyEnterRoomRequest> _logger;

        public ChatLobbyEnterRoomRequest(ILogger<ChatLobbyEnterRoomRequest> logger)
        {
            _logger = logger;
        }

        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {
            ushort chatLobbyId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[0..2]);
            session.ChatRoomId = chatLobbyId;
            var clientCount = session.Server.Sessions.Where(c => ((FragmentTcpSession)c).ChatRoomId == chatLobbyId).Count();
            var response = new ChatLobbyEnterRoomResponse().SetClientCount((ushort)clientCount).Build();

            //We have to send out a status update to all clients in this chat room but I don't understand where that comes from?

            return Task.FromResult<ICollection<FragmentMessage>>(new[] { response });
        }
    }
}
