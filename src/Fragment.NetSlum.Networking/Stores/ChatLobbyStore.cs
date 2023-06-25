using System;
using System.Collections.Generic;
using System.Linq;
using Fragment.NetSlum.Networking.Models;
using System.Text;
using System.Threading;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Stores;

public class ChatLobbyStore : IDisposable
{
    private readonly Dictionary<ushort, ChatLobbyModel> _chatLobbies = new();
    private readonly ReaderWriterLockSlim _rwLock = new();

    public IReadOnlyCollection<ChatLobbyModel> ChatLobbies
    {
        get
        {
            try
            {
                _rwLock.EnterReadLock();
                var lobbies = _chatLobbies.Values.ToArray();
                return lobbies;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
    }

    public void AddLobby(ChatLobbyModel lobby)
    {
        try
        {
            _rwLock.EnterWriteLock();
            _chatLobbies.TryAdd(lobby.LobbyId, lobby);
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    public bool TryUpdateLobby(ChatLobbyModel lobby, Action<ChatLobbyModel> updateAction)
    {
        var chatLobbyToUpdate = GetLobby(lobby.LobbyId);

        if (chatLobbyToUpdate == null)
        {
            return false;
        }

        try
        {
            _rwLock.EnterWriteLock();
            updateAction(lobby);

            return true;
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Gets a lobby by it's ID
    /// </summary>
    /// <param name="lobbyId"></param>
    /// <param name="lobbyType">When Any, the function will not check the type of lobby</param>
    /// <returns></returns>
    public ChatLobbyModel? GetLobby(ushort lobbyId, ChatLobbyType lobbyType = ChatLobbyType.Any)
    {
        try
        {
            _rwLock.EnterReadLock();

            _chatLobbies.TryGetValue(lobbyId, out var value);

            if (lobbyType == ChatLobbyType.Any)
            {
                return value;
            }

            return value?.LobbyType != lobbyType ? null : value;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    public ChatLobbyModel? GetLobbyBySession(FragmentTcpSession session)
    {
        try
        {
            _rwLock.EnterReadLock();

            foreach (var lobby in _chatLobbies.Values)
            {
                var player = lobby.GetPlayerByCharacterId(session.CharacterId);

                if (player != null)
                {
                    return lobby;
                }
            }
        }
        finally
        {
            _rwLock.ExitReadLock();
        }

        return null;
    }

    public void RemoveChatLobbyById(ushort id)
    {
        try
        {
            _rwLock.EnterWriteLock();
            _chatLobbies.Remove(id, out _);
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder("============ Chat Lobby Store ============");

        foreach (var (_, cb) in _chatLobbies)
        {
            sb.AppendLine(cb.ToString());
            sb.AppendLine("--------");
        }

        sb.AppendLine("==============================================");

        return sb.ToString();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _rwLock.Dispose();
    }
}
