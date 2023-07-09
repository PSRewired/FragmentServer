using System;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Networking.Stores;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.ChatLobby;

[FragmentPacket(MessageType.Data, OpCodes.DataLobbyEnterRoomRequest)]
public class ChatLobbyEnterRoomRequest : BaseRequest
{
    private readonly ILogger<ChatLobbyEnterRoomRequest> _logger;
    private readonly ChatLobbyStore _chatLobbyStore;
    private readonly FragmentContext _database;

    public ChatLobbyEnterRoomRequest(ILogger<ChatLobbyEnterRoomRequest> logger, ChatLobbyStore chatLobbyStore, FragmentContext database)
    {
        _logger = logger;
        _chatLobbyStore = chatLobbyStore;
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort chatLobbyId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);
        ChatLobbyType chatType = (ChatLobbyType)BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[2..4]);
        string roomPassword = request.Data.Span[4..].ToShiftJisString();

        _chatLobbyStore.RemoveSession(session);
        var chatLobby = GetOrCreateLobby(chatLobbyId, chatType);

        // Send the current client count
        var responses = new List<FragmentMessage>
        {
            new ChatLobbyEnterRoomResponse()
                .SetClientCount(chatLobby.PlayerCount)
                .Build()
        };

        if (chatType == ChatLobbyType.Chatroom && roomPassword != chatLobby.Password)
        {
            //TODO: Fix this to return an error response instead of disconnecting the player
            //throw new AuthenticationException(
                //$"Invalid password specified by {session.CharacterInfo!.CharacterName} while entering room {chatLobby.LobbyName}");
            return Task.FromResult<ICollection<FragmentMessage>>(responses);
        }


        foreach (var player in chatLobby.GetPlayers())
        {
            responses.Add(new ChatLobbyStatusUpdateResponse()
                .SetLastStatus(player.LastStatus)
                .SetPlayerIndex(player.PlayerIndex)
                .Build());
        }

        var myPlayer = new ChatLobbyPlayer(session);
        chatLobby.AddPlayer(myPlayer);

        _logger.LogWarning("Player {PlayerName} has entered {LobbyType} lobby {LobbyName} at player slot {PlayerIndex}",
            myPlayer.PlayerName, chatType, chatLobby.LobbyName, myPlayer.PlayerIndex);

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }

    /// <summary>
    /// Attempts to look up the lobby by ID and type. If not found, based on the specified type, will attempt to create a new instance
    /// of the applicable lobby.
    /// </summary>
    /// <param name="lobbyId"></param>
    /// <param name="lobbyType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Failed to find an acceptable lobby ID for the given type</exception>
    /// <exception cref="ArgumentOutOfRangeException">Invalid <paramref name="lobbyType"/> given</exception>
    private ChatLobbyModel GetOrCreateLobby(ushort lobbyId, ChatLobbyType lobbyType)
    {
        var chatLobby = _chatLobbyStore.GetLobby(lobbyId, lobbyType);

        if (chatLobby != null)
        {
            return chatLobby;
        }

        _logger.LogInformation("Chat lobby {LobbyId} ({LobbyType}) does not exist. Attempting to create it", lobbyId, lobbyType);

        var newLobby = lobbyType switch
        {
            ChatLobbyType.Guild => CreateGuildLobby(lobbyId),
            ChatLobbyType.Default => CreateDefaultLobby(lobbyId),
            ChatLobbyType.Chatroom => CreateDefaultLobby(lobbyId, lobbyType),
            _ => throw new ArgumentOutOfRangeException(nameof(lobbyType), lobbyType, null)
        };

        _chatLobbyStore.AddLobby(newLobby);

        return newLobby;
    }

    private ChatLobbyModel CreateDefaultLobby(ushort lobbyId, ChatLobbyType chatLobbyType = ChatLobbyType.Default)
    {
        var defaultLobby = _database.ChatLobbies.FirstOrDefault(l => l.Id == lobbyId && l.LobbyType == chatLobbyType);
        if (defaultLobby == null)
        {
            throw new ArgumentException($"Could not create chat lobby. {lobbyId} is not a valid default ID");
        }

        return new ChatLobbyModel((ushort)defaultLobby.Id, defaultLobby.ChatLobbyName);
    }

    private ChatLobbyModel CreateGuildLobby(ushort lobbyId)
    {
        var guild = _database.Guilds.FirstOrDefault(g => g.Id == lobbyId);

        if (guild == null)
        {
            throw new ArgumentException($"Could not create guild lobby. {lobbyId} is not a valid guild.");
        }

        return new ChatLobbyModel(guild.Id, guild.Name, ChatLobbyType.Guild);
    }
}
