using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.ChatLobby;
using Fragment.NetSlum.Networking.Sessions;
using Serilog;

namespace Fragment.NetSlum.Networking.Models;

public class ChatLobbyPlayer
{
    protected internal ushort PlayerIndex { get; set; }
    public int PlayerCharacterId => TcpSession.CharacterId;
    public string? PlayerName => TcpSession.CharacterInfo?.CharacterName;
    public FragmentTcpSession TcpSession { get; }
    public ChatLobbyModel ChatLobby { get; set; } = null!;

    public Memory<byte> LastStatus { get; private set; } = Array.Empty<byte>();
    private ILogger Logger => Log.ForContext<ChatLobbyPlayer>();

    public ChatLobbyPlayer(FragmentTcpSession session)
    {
        TcpSession = session;
    }

    public void Send(List<FragmentMessage> messages)
    {
        Log.ForContext<ChatLobbyPlayer>().Debug("Sending data to player {PlayerName} at index {PlayerIndex}\n{HexDump}", PlayerName,
            PlayerIndex, string.Join('\n', messages.Select(m => m.Data.ToHexDump())));

        TcpSession.Send(messages);
    }

    /// <summary>
    /// Updates the internal status metadata for this player. The information will then be broadcast out to all clients
    /// </summary>
    /// <param name="status"></param>
    public void UpdateStatus(Memory<byte> status)
    {
        //TODO: Something is buggy and is causing this to return 6 characters too many.
        status = status[..^6];
        var statusResponse = new ChatLobbyStatusUpdateResponse()
            .SetPlayerIndex(PlayerIndex)
            .SetLastStatus(status)
            .Build();

        LastStatus = status;

        ChatLobby.NotifyAllExcept(this, statusResponse);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"PlayerIndex: {PlayerIndex}");
        sb.AppendLine($"Character ID: {PlayerCharacterId}");
        sb.AppendLine($"Character Name: {PlayerName}");

        return sb.ToString();
    }
}
