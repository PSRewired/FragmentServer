using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Networking.Extensions;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Persistence.Interceptors;
using Fragment.NetSlum.Persistence.Listeners;
using Fragment.NetSlum.Server.Converters;
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
using Serilog;

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
            .Configure<JsonSerializerOptions>(opt =>
            {
                opt.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })
            .AddRouting(opt => opt.LowercaseUrls = true)
            .AddEndpointsApiExplorer()
            .AddControllers();

        services
            .AddDbContext<FragmentContext>((provider, opt) =>
            {
                opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                opt.AddInterceptors(provider.GetRequiredService<EntityChangeInterceptor>());
            })
            .AddAutoMigrations<FragmentContext>();

        services.UseEntityListener()
            .AddListener<TimestampableEntityListener>()
            .AddListener<CharacterStatsChangeListener>();

        services.AddHealthChecks()
            .AddDbContextCheck<FragmentContext>()
            .AddPrivateMemoryHealthCheck(2147483648)
            .AddProcessAllocatedMemoryHealthCheck(2048)
        ;

        // Register command bus
        services.AddMediator(opt =>
        {
            opt.ServiceLifetime = ServiceLifetime.Transient;
        });
        services.AddScoped<ICommandBus, MediatorCommandBus>();

        services.AddOpenApiDocument(doc =>
        {
            doc.Version = "v1";
            doc.Title = "Fragment.Netslum";
            doc.Description = "REST API that provides information from the .hack//Fragment server";
        });
        services.AddCors();

        services.AddSingleton<ImageConverter>();

        services.AddPacketHandling();
        services.Configure<ServerConfiguration>(Configuration.GetSection("TcpServer"));
        services.AddSingleton<ITcpServer, Servers.Server>();
        services.AddSingleton(typeof(ChatLobbyStore));
        services.AddHostedService<ServerBackgroundService>();
        services.AddHostedService<ClientTickService>();
        services.AddHostedService<ChatLobbyBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseRouting();
        app.UseCors(opt =>
        {
            opt.AllowAnyHeader();
            opt.AllowAnyMethod();
            opt.AllowAnyOrigin();
        });

        app.UseHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        app.UseStaticFiles();
        app.UseOpenApi();
        app.UseSwaggerUi(opt =>
        {
            opt.Path = "/api/docs";
            opt.CustomStylesheetPath = "/swagger/css/ui.css";
        });
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        // Register additional encodings, since dotnet core only supports ASCII, ISO-8859, and UTF by default
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Log.ForContext<Program>().Warning("Server is live!");
    }
}
