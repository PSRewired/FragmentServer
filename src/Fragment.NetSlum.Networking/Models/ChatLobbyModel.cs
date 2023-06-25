using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using ILogger = Serilog.ILogger;

namespace Fragment.NetSlum.Networking.Models;

public class ChatLobbyModel
{
    private const ushort MaxPlayers = 255;

    public ushort LobbyId { get; private set; }
    public string LobbyName { get; private set; }
    public ushort PlayerChatLobbyId { get; private set; }
    public ChatLobbyType LobbyType { get; set; }

    private readonly ChatLobbyPlayer?[] _chatLobbyPlayers;
    private static ILogger Log => Serilog.Log.ForContext<ChatLobbyModel>();
    public ushort PlayerCount => (ushort)GetPlayers().Length;
    private readonly Semaphore _playerIdxLock = new(1, 1);

    public ChatLobbyModel(ushort id, string name, ChatLobbyType lobbyType = ChatLobbyType.Default)
    {
        LobbyId = id;
        LobbyName = name;
        _chatLobbyPlayers = new ChatLobbyPlayer[MaxPlayers + 1];
        LobbyType = lobbyType;
    }


    public static ChatLobbyModel FromEntity(DefaultLobby lobbyEntity)
    {
        return new ChatLobbyModel((ushort)lobbyEntity.Id, lobbyEntity.DefaultLobbyName);
    }

    public int AddPlayer(ChatLobbyPlayer player)
    {
        try
        {
            _playerIdxLock.WaitOne();
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
    public ChatLobbyPlayer[] GetPlayers()
    {
        return _chatLobbyPlayers.Where(p => p != null).ToArray()!;
    }

    public ChatLobbyPlayer? GetPlayer(int idx)
    {
        return _chatLobbyPlayers.FirstOrDefault(p => p?.PlayerIndex == idx);
    }

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
        NotifyAllExcept(sender, new List<FragmentMessage> { message });
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
        SendTo(idx, new List<FragmentMessage> {message});
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
        sb.AppendLine($"===== Chat Lobby: {LobbyName} ({LobbyId} =====");
        sb.AppendLine($"Player ChatRoom Id: {PlayerChatLobbyId}");
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
