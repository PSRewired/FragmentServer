using System.Data;
using System.Text;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Extensions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Persistence.Listeners;
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
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("Database");

        // Add failsafe to ensure the database is never executed on the original one
        if (connectionString!.Contains("Database=fragment;"))
        {
            throw new ConstraintException("Auto-migrations have been disabled. The connection string contains the old database information!");
        }

        services
            .AddDbContext<FragmentContext>(opt => opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)))
            .AddAutoMigrations<FragmentContext>();

        services.UseEntityListener()
            .AddListener<TimestampableEntityListener>()
            .AddListener<CharacterStatsChangeListener>();

        services.AddHealthChecks()
            .AddDbContextCheck<FragmentContext>()
            .AddPrivateMemoryHealthCheck(2147483648)
            .AddProcessAllocatedMemoryHealthCheck(2048)
        ;

        services.AddCommandBus(typeof(Startup), typeof(Networking.Entrypoint));
        services.AddAutoMapper(typeof(Startup));

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
