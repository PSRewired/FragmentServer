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
    /// <summary>
    /// The slot index that this player currently holds
    /// </summary>
    protected internal ushort PlayerIndex { get; set; }

    /// <summary>
    /// The character ID associated with this lobby player
    /// </summary>
    public int PlayerCharacterId => TcpSession.CharacterId;

    /// <summary>
    /// The character name associated with this lobby player
    /// </summary>
    public string? PlayerName => TcpSession.CharacterInfo?.CharacterName;

    public ChatLobbyModel ChatLobby { get; set; } = null!;

    /// <summary>
    /// Holds the current status that we've received from this player
    /// </summary>
    public Memory<byte> LastStatus { get; set; } = Array.Empty<byte>();

    public DateTime LastStatusUpdateReceivedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// When a guild invite is sent to this player, this field is populated with the guild ID that
    /// the invite came from
    /// </summary>
    public ushort? CurrentGuildEnticementId { get; set; }

    /// <summary>
    /// Timestamp that signifies when this player joined the lobby
    /// </summary>
    public DateTime JoinedAt = DateTime.UtcNow;

    private FragmentTcpSession TcpSession { get; }

    public ChatLobbyPlayer(FragmentTcpSession session)
    {
        TcpSession = session;
    }

    /// <summary>
    /// Sends messages to this player
    /// </summary>
    /// <param name="messages"></param>
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
        LastStatusUpdateReceivedAt = DateTime.UtcNow;

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
