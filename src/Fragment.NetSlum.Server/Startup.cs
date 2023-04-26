using System.Text;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Extensions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Servers;
using Fragment.NetSlum.Server.Services;
using Fragment.NetSlum.TcpServer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
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
        services.AddDbContext<FragmentContext>(opt =>
        {
            var connectionString = Configuration.GetConnectionString("Database");
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        services.AddHealthChecks()
            .AddDbContextCheck<FragmentContext>()
            .AddPrivateMemoryHealthCheck(2147483648)
            .AddProcessAllocatedMemoryHealthCheck(2048)
        ;

        services.AddCommandBus(typeof(Startup), typeof(Networking.Entrypoint));
        services.AddPacketHandling();
        services.Configure<ServerConfiguration>(Configuration.GetSection("TcpServer"));
        services.AddSingleton<ITcpServer, Servers.Server>();
        services.AddHostedService<ServerBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        // Register additional encodings, since dotnet core only supports ASCII, ISO-8859, and UTF by default
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
}
