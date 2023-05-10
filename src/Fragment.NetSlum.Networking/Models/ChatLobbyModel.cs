using Fragment.NetSlum.Persistence.Entities;
using MediatR;
using Serilog;
using System.Xml.Linq;

namespace Fragment.NetSlum.Networking.Models
{
    public class ChatLobbyModel
    {
        public ChatLobbies ChatLobby;
        private ushort MaxPlayers = 255;
        private readonly ChatLobbyPlayerModel[] ChatLobbyPlayers;
       
        public ushort PlayerCount => (ushort)GetPlayers().Length;
        private readonly Semaphore playerIdxLock = new(1, 1);

        public ChatLobbyModel()
        {
            ChatLobbyPlayers = new ChatLobbyPlayerModel[MaxPlayers];
        }

        public int AddPlayer(ChatLobbyPlayerModel player)
        {

            try
            {
                playerIdxLock.WaitOne();
                var idx = GetAvailablePlayerIndex();
                player.PlayerIndex = idx;

                ChatLobbyPlayers[idx] = player;
                return idx;
            }
            finally
            {
                playerIdxLock.Release();
            }
        }
        public ChatLobbyPlayerModel[] GetPlayers()
        {
            return ChatLobbyPlayers.Where(p => p != null).ToArray();
        }
        public void RemovePlayer(ChatLobbyPlayerModel player)
        {
            if (player == null)
            {
                return;
            }

            playerIdxLock.WaitOne();

            try
            {
                // If for some reason this player has multiple connections, we need to remove them all
                for (var pIdx = 0; pIdx < ChatLobbyPlayers.Length; pIdx++)
                {
                    var chatPlayer = ChatLobbyPlayers[pIdx];

                    if (chatPlayer?.PlayerAccountId != player.PlayerAccountId)
                    {
                        continue;
                    }

                    ChatLobbyPlayers[pIdx] = null;
                }
            }
            finally
            {
                playerIdxLock.Release();
            }
        }
        public ChatLobbyPlayerModel GetPlayerByAccountId(uint accountId)
        {
            foreach (var player in ChatLobbyPlayers)
            {
                if(player?.PlayerAccountId == accountId)
                {
                    return player;
                }
            }
            return null;
        }

        private ushort GetAvailablePlayerIndex()
        {
            for (ushort i = 0; i < ChatLobbyPlayers.Length; i++)
            {
                if (ChatLobbyPlayers[i] == null)
                {
                    return i;
                }
            }
            throw new Exception("Chat lobby is full!");
        }

    }
}
