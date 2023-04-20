using Fragment.NetSlum.Core.DependencyInjection;
using Fragment.NetSlum.TcpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Sessions;

public class FragmentTcpSession : TcpSession, IScopeable
{
    public IServiceScope ServiceScope { get; }
    private readonly ILogger<FragmentTcpSession> _logger;

    public FragmentTcpSession(ITcpServer server, IServiceScope serviceScope) : base(server)
    {
        ServiceScope = serviceScope;
        _logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<FragmentTcpSession>>();
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
