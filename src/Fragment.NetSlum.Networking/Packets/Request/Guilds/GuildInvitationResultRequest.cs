using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Persistence;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildInvitationResponse)]
public class GuildInvitationResultRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ChatLobbyStore _chatLobbyStore;
    private readonly ILogger<GuildInvitationResultRequest> _logger;

    public GuildInvitationResultRequest(FragmentContext database, ChatLobbyStore chatLobbyStore, ILogger<GuildInvitationResultRequest> logger)
    {
        _database = database;
        _chatLobbyStore = chatLobbyStore;
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort responseCode = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        // Guild invite was accepted
        if (responseCode == 0x7608)
        {
            var myChatLobby = _chatLobbyStore.GetLobbyBySession(session);
            var myLobbyPlayer = myChatLobby?.GetPlayerByCharacterId(session.CharacterId);

            if (myLobbyPlayer?.CurrentGuildEnticementId == null)
            {
                throw new DataException($"Current player/lobby/guild reference could not be found while trying to accept a guild invite");
            }

            var myCharacter = _database.Characters.First(c => c.Id == session.CharacterId);
            var guild = _database.Guilds
                .First(g => g.Id == myLobbyPlayer.CurrentGuildEnticementId);

            guild.Members.Add(myCharacter);
            _database.SaveChanges();
            myLobbyPlayer.CurrentGuildEnticementId = null;

            _logger.LogInformation("Successfully added member {CharacterName} to guild {GuildName}", myCharacter.CharacterName, guild.Name);
        }

        return SingleMessage(new GuildInvitationResultConfirmationResponse(responseCode)
            .Build());
    }
}
