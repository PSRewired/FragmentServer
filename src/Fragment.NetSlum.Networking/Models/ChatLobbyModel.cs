using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using ILogger = Serilog.ILogger;

namespace Fragment.NetSlum.Networking.Models;

public class ChatLobbyModel
{
    private const ushort MaxPlayers = 255;

    /// <summary>
    /// The ID assigned to this lobby when it was created
    /// </summary>
    public ushort LobbyId { get; protected internal set; }

    /// <summary>
    /// The name designated to this lobby during creation
    /// </summary>
    public string LobbyName { get; }

    /// <summary>
    /// Designates whether this is a general or private lobby type
    /// </summary>
    public ChatLobbyType LobbyType { get; set; }

    /// <summary>
    /// The current number of active players within this lobby
    /// </summary>
    public ushort PlayerCount => (ushort)GetPlayers().Length;

    /// <summary>
    /// The password required to enter the room. Defaults to <see cref="string.Empty" />
    /// </summary>
    public string Password { get; set; } = "1234";

    public ChatLobbyModel? ParentChatLobby { get; init; }

    private readonly ChatLobbyPlayer?[] _chatLobbyPlayers;
    private static ILogger Log => Serilog.Log.ForContext<ChatLobbyModel>();
    private readonly Semaphore _playerIdxLock = new(1, 1);

    public ChatLobbyModel(string name, ChatLobbyType lobbyType = ChatLobbyType.Default)
    {
        LobbyId = 0;
        LobbyName = name;
        _chatLobbyPlayers = new ChatLobbyPlayer[MaxPlayers + 1];
        LobbyType = lobbyType;
    }

    public ChatLobbyModel(ushort id, string name, ChatLobbyType lobbyType = ChatLobbyType.Default)
    {
        LobbyId = id;
        LobbyName = name;
        _chatLobbyPlayers = new ChatLobbyPlayer[MaxPlayers + 1];
        LobbyType = lobbyType;
    }

    /// <summary>
    /// Adds a new player to this lobby and returns the player index that was assigned to them
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int AddPlayer(ChatLobbyPlayer player)
    {
        try
        {
            _playerIdxLock.WaitOne();

            // If this player already exists, just update their record at the current index
            if (GetPlayerByCharacterId(player.PlayerCharacterId) != null)
            {
                return player.PlayerIndex;
            }

            var idx = GetAvailablePlayerIndex();
            player.PlayerIndex = idx;
            player.ChatLobby = this;

            _chatLobbyPlayers[idx] = player;
            return idx;
        }
        finally
        {
            Log.Information("Added player {PlayerName} at index {PlayerIndex} to chat lobby {LobbyName}({LobbyId})", player.PlayerName, player.PlayerIndex, LobbyName, LobbyId);
            _playerIdxLock.Release();
        }
    }

    /// <summary>
    /// Returns all active <see cref="ChatLobbyPlayer"/> instances in this lobby
    /// </summary>
    /// <returns></returns>
    public ChatLobbyPlayer[] GetPlayers()
    {
        return _chatLobbyPlayers.Where(p => p != null).ToArray()!;
    }

    /// <summary>
    /// Attempts to retrieve a player by their lobby index
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public ChatLobbyPlayer? GetPlayer(int idx)
    {
        return _chatLobbyPlayers.FirstOrDefault(p => p?.PlayerIndex == idx);
    }

    /// <summary>
    /// Removes an player instance from the current lobby
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayer(ChatLobbyPlayer player)
    {
        _playerIdxLock.WaitOne();

        try
        {
            // If for some reason this player has multiple connections, we need to remove them all
            for (var pIdx = 0; pIdx < _chatLobbyPlayers.Length; pIdx++)
            {
                var chatPlayer = _chatLobbyPlayers[pIdx];

                if (chatPlayer?.PlayerCharacterId != player.PlayerCharacterId)
                {
                    continue;
                }

                _chatLobbyPlayers[pIdx] = null;

                NotifyAllExcept(null,
                    new ClientLeftChatLobbyResponse(chatPlayer.PlayerIndex).Build());
            }
        }
        finally
        {
            Log.Information("Removed player {PlayerName} at index {PlayerIndex} from chat lobby {LobbyName}({LobbyId})", player.PlayerName, player.PlayerIndex, LobbyName, LobbyId);
            _playerIdxLock.Release();
        }
    }

    /// <summary>
    /// Looks up an active player by their character ID
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public ChatLobbyPlayer? GetPlayerByCharacterId(int accountId)
    {
        foreach (var player in _chatLobbyPlayers)
        {
            if(player?.PlayerCharacterId == accountId)
            {
                return player;
            }
        }

        return null;
    }

    /// <summary>
    /// Sends message data to all players in the room except for the sender
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="messages"></param>
    public void NotifyAllExcept(ChatLobbyPlayer? sender, List<FragmentMessage> messages)
    {
        foreach (var player in _chatLobbyPlayers)
        {
            if (player == null || player.PlayerIndex == sender?.PlayerIndex)
            {
                continue;
            }

            player.Send(messages);
        }
    }

    /// <summary>
    /// Sends message data to all players in the room except for the player specified.
    /// If <paramref name="sender"/> is null, the message is broadcast to everyone
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    public void NotifyAllExcept(ChatLobbyPlayer? sender, FragmentMessage message)
    {
        Log.ForContext<ChatLobbyModel>().Debug("Notifying lobby {LobbyName} ({LobbyId}) with data:\n{HexDump}", LobbyName, LobbyId, message.Data.ToHexDump());
        NotifyAllExcept(sender, [message]);
    }

    /// <summary>
    /// Sends message data to the given player index
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="messages"></param>
    public void SendTo(ushort idx, List<FragmentMessage> messages)
    {
        foreach (var player in _chatLobbyPlayers)
        {
            if (player?.PlayerIndex != idx)
            {
                continue;
            }

            player.Send(messages);
            break;

        }
    }

    /// <summary>
    /// Sends message data to the given player index
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="message"></param>
    public void SendTo(ushort idx, FragmentMessage message)
    {
        SendTo(idx, [message]);
    }

    private ushort GetAvailablePlayerIndex()
    {
        for (ushort i = 0; i < _chatLobbyPlayers.Length; i++)
        {
            if (_chatLobbyPlayers[i] == null)
            {
                return i;
            }
        }
        throw new Exception("Chat lobby is full!");
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"===== Chat Lobby: {LobbyName} ({LobbyId}) =====");
        sb.AppendLine($"Lobby Type: {LobbyType}");
        sb.AppendLine($"Player Count: {PlayerCount}");
        sb.AppendLine("Player List:");

        foreach (var player in _chatLobbyPlayers.Where(p => p != null))
        {
            sb.AppendLine($"\t{player!.ToString()}");
        }

        return sb.ToString();
    }
}
