using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataSendGuildInvite)]
public class SendGuildInviteRequest : BaseRequest
{
    private readonly ChatLobbyStore _chatLobbyStore;
    private readonly ILogger<SendGuildInviteRequest> _logger;

    public SendGuildInviteRequest(ChatLobbyStore chatLobbyStore, ILogger<SendGuildInviteRequest> logger)
    {
        _chatLobbyStore = chatLobbyStore;
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var playerToInviteIdx = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        var myChatLobby = _chatLobbyStore.GetLobbyBySession(session);

        var myPlayer = myChatLobby?.GetPlayerByCharacterId(session.CharacterId);
        var playerToInvite = myChatLobby?.GetPlayer(playerToInviteIdx);

        if (myPlayer == null)
        {
            throw new DataException("Session could not be found in the requested chat lobby, or lobby does not exist");
        }

        if (playerToInvite == null)
        {
            _logger.LogWarning(
                "Failed to invite player at index {PlayerIndex} in lobby {LobbyName}. The lobby or player index does not exist",
                playerToInviteIdx, myChatLobby?.LobbyName);

            return NoResult();
        }

        _logger.LogInformation("Guild request sent from {SenderName} to player {RecipientName} in chat lobby {ChatLobbyName}",
            myPlayer.PlayerName, playerToInvite.PlayerName, myChatLobby!.LobbyName);

        myChatLobby.SendTo(playerToInviteIdx, new InvitePlayerToGuildResponse(myPlayer.PlayerIndex).Build());

        return NoResult();
    }
}
