using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Misc;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;

namespace Fragment.NetSlum.Networking.Packets.Request.Misc;

[FragmentPacket(MessageType.Data, OpCodes.PrivateBroadcast)]
public class PrivateBroadcastRequest : BaseRequest
{
    private readonly ChatLobbyStore _chatLobbyStore;

    public PrivateBroadcastRequest(ChatLobbyStore chatLobbyStore)
    {
        _chatLobbyStore = chatLobbyStore;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var lobbyId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);
        var destinationPlayerIndex = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[2..4]);
        var messageData = request.Data[4..];

        var chatLobby = _chatLobbyStore.GetLobby(lobbyId);

        var myLobbySession = chatLobby?.GetPlayerByCharacterId(session.CharacterId);
        var recipient = chatLobby?.GetPlayer(destinationPlayerIndex);

        var response = new PrivateBroadcastResponse()
            .SetData(messageData)
            .SetLobbyId(lobbyId);

        if (myLobbySession == null)
        {
            throw new DataException($"Player {session.CharacterId} does not exist in the room {lobbyId} to send a PM");
        }

        if (recipient != null)
        {
            chatLobby?.SendTo(recipient.PlayerIndex, response.SetSenderIndex(myLobbySession.PlayerIndex).Build());
        }

        return SingleMessage(response.SetIsSender(true).Build());
    }
}
