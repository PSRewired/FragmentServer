using Fragment.NetSlum.Networking.Models;
using System.Text;

namespace Fragment.NetSlum.Networking.Stores;

public class ChatLobbyStore :IDisposable
{
    private readonly Dictionary<ushort, ChatLobbyModel> _chatLobbies = new();
    private readonly ReaderWriterLockSlim _rwLock = new();
    private ushort _gameCount;

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
            _chatLobbies.TryAdd(_gameCount, lobby);
            _gameCount++;
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    public bool TryUpdateLobby(ChatLobbyModel lobby, Action<ChatLobbyModel> updateAction)
    {
        var chatLobbyToUpdate = GetLobby((ushort)lobby.LobbyId);

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
    public ChatLobbyModel? GetLobby(ushort lobbyId)
    {
        try
        {
            _rwLock.EnterReadLock();

            if (_chatLobbies.TryGetValue(lobbyId, out var value))
            {
                return value;
            }

            return _chatLobbies.FirstOrDefault(c => c.Key == lobbyId).Value;

        }
        finally
        {
            _rwLock.ExitReadLock();
        }
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
        _rwLock?.Dispose();
    }
}
