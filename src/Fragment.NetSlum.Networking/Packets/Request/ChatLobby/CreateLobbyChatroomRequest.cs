using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(MessageType.Data, OpCodes.DataLobbyChatroomProtectedCreate)]
[FragmentPacket(MessageType.Data, OpCodes.DataLobbyChatroomOpenCreate)]
public class CreateLobbyChatroomRequest : BaseRequest
{
    private readonly ChatLobbyStore _chatLobbyStore;

    public CreateLobbyChatroomRequest(ChatLobbyStore chatLobbyStore)
    {
        _chatLobbyStore = chatLobbyStore;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);

        //var entryId = reader.ReadUInt16();
        reader.Skip(2); // This would normally contain the "slot" of the new chat lobby, however in this server it will always be 0 since we auto-generate ids
        var roomNameBytes = reader.ReadString(out _).ToShiftJisString();
        var roomPasswordBytes = reader.ReadString(out _).ToShiftJisString();

        var newLobby = new ChatLobbyModel(roomNameBytes, ChatLobbyType.Chatroom)
        {
            Password = roomPasswordBytes,
            ParentChatLobby = _chatLobbyStore.GetLobbyBySession(session, ChatLobbyType.Default),
        };

        _chatLobbyStore.AddLobby(newLobby);

        return SingleMessage(new CreateLobbyChatroomResponse(newLobby.LobbyId).Build());
    }
}
