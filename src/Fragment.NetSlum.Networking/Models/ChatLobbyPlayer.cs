using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Sessions;
using Serilog;

namespace Fragment.NetSlum.Networking.Models;

public class ChatLobbyPlayer
{
    public ushort PlayerIndex { get; set; }
    public int PlayerAccountId => TcpSession.PlayerAccountId;
    public string? PlayerName => TcpSession.CharacterInfo?.CharacterName;
    public FragmentTcpSession TcpSession { get; }
    public ChatLobbyModel ChatLobby { get; set; } = null!;
    private ILogger Logger => Log.ForContext<ChatLobbyPlayer>();

    public ChatLobbyPlayer(FragmentTcpSession session)
    {
        TcpSession = session;
    }

    public void Send(List<FragmentMessage> messages)
    {
        TcpSession.Send(messages);
    }
}
