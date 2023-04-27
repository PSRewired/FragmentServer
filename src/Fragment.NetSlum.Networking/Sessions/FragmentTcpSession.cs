using System.Runtime.CompilerServices;
using Fragment.NetSlum.Core.DependencyInjection;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Core.Models;
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

    public byte[] IpAddress { get; set; } = new byte[8];
    public byte[] Port { get; set; } = new byte[2];
    public bool IsAreaServer { get; set; } = false;
    public DateTime LastContacted { get; set; } = DateTime.UtcNow;

    //Fields only used for a Player
    public int PlayerAccountId { get; set; }
    public int ChatRoomId { get; set; }
    public int GuildId { get; set; }
    public CharacterInfo? CharacterInfo { get; set; }

    //Area Server Fields
    public byte[] AreaServerName { get; set; } = Array.Empty<byte>();
    public ushort AreaServerLevel { get; set; }
    public byte AreaServerStatus { get; set; }
    public ushort AreaServerPlayerCount { get;set; }



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
            _logger.LogDebug("[{ClsName}] Sending packet\n{LineBreak1}\n{HexDump}\n{LineBreak2}",
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
