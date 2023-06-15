using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.TcpServer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fragment.NetSlum.Server.Servers;

public class Server : TcpServer.TcpServer, IServer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public IFeatureCollection Features { get; } = new FeatureCollection();

    public Server(IOptions<ServerConfiguration> serverOptions, IServiceScopeFactory serviceScopeFactory) : base(serverOptions.Value)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override TcpSession CreateSession()
    {
        var scope = _serviceScopeFactory.CreateScope();

        return new FragmentTcpSession(this, scope);
    }

    public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken) where TContext : notnull
    {
        Start();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Stop();

        return Task.CompletedTask;
    }
}
