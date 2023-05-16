using System.Runtime.CompilerServices;
using Fragment.NetSlum.Core.DependencyInjection;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Pipeline;
using Fragment.NetSlum.TcpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Sessions;

public class FragmentTcpSession : TcpSession, IScopeable
{
    public IServiceScope ServiceScope { get; }
    private readonly ILogger<FragmentTcpSession> _logger;
    private readonly FragmentPacketPipeline<FragmentTcpSession> _packetPipeline;

    public bool IsAreaServer => AreaServerInfo != null;
    public AreaServerInformation? AreaServerInfo { get; set; }

    public DateTime LastContacted { get; set; } = DateTime.UtcNow;

    //Fields only used for a Player
    public int PlayerAccountId { get; set; }
    public int GuildId { get; set; }

    public CharacterInfo? CharacterInfo { get; set; }
    public byte[] LastStatus { get; set; } = Array.Empty<byte>();


    public FragmentTcpSession(ITcpServer server, IServiceScope serviceScope) : base(server)
    {
        ServiceScope = serviceScope;
        _packetPipeline = serviceScope.ServiceProvider.GetRequiredService<FragmentPacketPipeline<FragmentTcpSession>>();
        _logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<FragmentTcpSession>>();
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
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
       Send(_packetPipeline.Encode(data, CancellationToken.None).Span);
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation("Session {Id} disconnected or expired", Id);
        base.OnDisconnected();

        try
        {
            _logger.LogDebug("Disposing service scope for {ClassName}", GetType().Name);
            ServiceScope.Dispose();
        }
        catch (ObjectDisposedException) { } // Catch and ignore error if the scope is already disposed
    }
}
