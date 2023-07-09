using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.DependencyInjection;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Pipeline;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.TcpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Sessions;

public class FragmentTcpSession : TcpSession, IScopeable
{
    public IServiceScope ServiceScope { get; }
    private readonly ILogger<FragmentTcpSession> _logger;
    private readonly FragmentPacketPipeline<FragmentTcpSession> _packetPipeline;

    /// <summary>
    /// Timer that monitors the packet aggregation interval for this session
    /// </summary>
    private Stopwatch LastFlushTimer { get; } = Stopwatch.StartNew();

    /// <summary>
    /// The number of milliseconds elapsed since the last time data was flushed out to this session.
    /// </summary>
    public long ElapsedMillisecondsSinceLastFlush => LastFlushTimer.ElapsedMilliseconds;

    //Fields for area server sessions
    /// <summary>
    /// Dictates that this session has signaled that it is an area server instead of a player session
    /// </summary>
    public bool IsAreaServer => AreaServerInfo != null;

    /// <summary>
    /// Metadata associated with the area server that this session is currently hosting
    /// </summary>
    public AreaServerInformation? AreaServerInfo { get; set; }

    /// <summary>
    /// Timestamp that represents the last time we received a ping from this session
    /// </summary>
    public DateTime LastContacted { get; set; } = DateTime.UtcNow;

    //Fields only used for a Player
    /// <summary>
    /// The active "save" ID that is associated to this session
    /// </summary>
    public int PlayerAccountId { get; set; }

    /// <summary>
    /// The character reference that belongs to this session
    /// </summary>
    public int CharacterId { get; set; }

    /// <summary>
    /// Metadata for the character that is currently assigned to this session
    /// </summary>
    public CharacterInfo? CharacterInfo { get; set; }


    public FragmentTcpSession(ITcpServer server, IServiceScope serviceScope) : base(server)
    {
        ServiceScope = serviceScope;
        _packetPipeline = serviceScope.ServiceProvider.GetRequiredService<FragmentPacketPipeline<FragmentTcpSession>>();
        _logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<FragmentTcpSession>>();
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    protected override async Task OnReceived(Memory<byte> data, CancellationToken cancellationToken)
    {
#if (DEBUG)
        _logger.LogDebug("Received packet\n{LineBreak1}\n{HexDump}\n{LineBreak2}",
            new string('=', 32),
            data.ToHexDump(),
            new string('=', 32)
        );
        _logger.LogDebug("Data Stream: {Stream}", data.Span.ToHexString());
#endif

        try
        {
            var resp = await _packetPipeline.Handle(this, data, cancellationToken);

            if (resp.Length <= 0)
            {
                return;
            }

#if (DEBUG)
            _logger.LogDebug("[{ClsName}] Sending packet to {SessionId}\n{LineBreak1}\n{HexDump}\n{LineBreak2}",
                Id,
                GetType().Name,
                new string('=', 32),
                resp.Span.ToHexDump(),
                new string('=', 32)
            );
#endif

            Send(resp.Span);
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug("Cancellation handled in TcpSession OnReceive callback");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Session {Id} encountered an unhandled exception. Disconnecting session", Id);
            _logger.LogCritical("Buffer Content: {Content}", data.ToHexDump());
            Disconnect();
        }
    }

    /// <summary>
    /// Sends data to this client, ensuring it has been encoded using the configured pipeline for them
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected internal void Send(List<FragmentMessage> data)
    {
        var respData = _packetPipeline.Encode(data, CancellationToken.None).Span;

        Send(respData);
        Flush();
    }

    /// <summary>
    /// Flush all available outgoing messages to the client and reset the elapsed milliseconds to zero
    /// </summary>
    public override void Flush()
    {
        base.Flush();
        LastFlushTimer.Restart();
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation("Session {Id} disconnected or expired", Id);
        base.OnDisconnected();

        try
        {
            ServiceScope.ServiceProvider.GetRequiredService<ChatLobbyStore>().RemoveSession(this);

            _logger.LogDebug("Disposing service scope for {ClassName}", GetType().Name);
            ServiceScope.Dispose();
        }
        catch (ObjectDisposedException)
        {
        } // Catch and ignore error if the scope is already disposed
    }
}
