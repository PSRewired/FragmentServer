using System;
using System.Collections.Generic;
using System.Linq;
using Fragment.NetSlum.Networking.Models;
using System.Text;
using System.Threading;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.DependencyInjection;

namespace Fragment.NetSlum.Networking.Stores;

public class ChatLobbyStore : IDisposable
{
    private readonly Dictionary<ushort, ChatLobbyModel> _chatLobbies = new();
    private readonly ReaderWriterLockSlim _rwLock = new();
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ChatLobbyStore(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

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
            lobby.ServiceScope = _serviceScopeFactory.CreateScope();
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

    public ChatLobbyModel? GetLobby(ushort lobbyId, ChatLobbyType lobbyType = ChatLobbyType.Default)
    {
        try
        {
            _rwLock.EnterReadLock();

            _chatLobbies.TryGetValue(lobbyId, out var value);

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
            _chatLobbies.Remove(id, out var lobby);
            lobby?.ServiceScope.Dispose();
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder("============ Chat Lobby Store ============");

        foreach (var (c, cb) in _chatLobbies)
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
