using Fragment.NetSlum.Networking.Extensions;
using Fragment.NetSlum.Server.Servers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fragment.NetSlum.Server;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddPacketHandling();
        services.Configure<ServerConfiguration>(Configuration.GetSection("TcpServer"));
        services.AddSingleton<IServer, Servers.Server>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {

    }
}
