using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Models;

public class ChatLobbyPlayer
{
    public ushort PlayerIndex { get; set; }
    public int PlayerAccountId => TcpSession.PlayerAccountId;
    public FragmentTcpSession TcpSession { get; }
    public ChatLobbyModel ChatLobby { get; set; } = null!;

    public ChatLobbyPlayer(FragmentTcpSession session)
    {
        TcpSession = session;
    }

    public void Send(List<FragmentMessage> messages)
    {
        TcpSession.Send(messages);
    }
}
