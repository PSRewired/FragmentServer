using System;
using System.Data;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Networking.Extensions;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Persistence.Interceptors;
using Fragment.NetSlum.Persistence.Listeners;
using Fragment.NetSlum.Server.Authentication;
using Fragment.NetSlum.Server.Authentication.Configuration;
using Fragment.NetSlum.Server.Converters;
using Fragment.NetSlum.Server.Servers;
using Fragment.NetSlum.Server.Services;
using Fragment.NetSlum.Server.Transformers;
using Fragment.NetSlum.TcpServer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ZiggyCreatures.Caching.Fusion;

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
            throw new ConstraintException(
                "Auto-migrations have been disabled. The connection string contains the old database information!");
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

        services.AddMemoryCache();
        services.AddFusionCache()
            .WithDefaultEntryOptions(opt =>
            {
                opt.Duration = TimeSpan.FromMinutes(5);
                opt.SkipBackplaneNotifications = true;
            })
            .WithMemoryBackplane(opt =>
            {
                opt.ConnectionId = Assembly.GetEntryAssembly()!.GetName().Name;
            });

        services.Configure<DiscordAuthOptions>(Configuration.GetSection("Authentication"));

        services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<DiscordAuthOptions>>((options, discordOptions) =>
            {
                options.TokenHandlers.Clear();
                options.TokenHandlers.Add(new DiscordJwtTokenHandler(discordOptions));

                options.Events = new JwtBearerEvents
                {
                    // Allow the JWT policy to accept the token from either the Authorization header, or the 'token' cookie
                    // supplied in the request
                    OnMessageReceived = context =>
                    {
                        context.Token = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var bearerToken)
                            ? AuthenticationHeaderValue.Parse(bearerToken.ToString()).Parameter
                            : context.HttpContext.Request.Cookies["netslum-token"];

                        return Task.CompletedTask;
                    },
                };

                options.MapInboundClaims = true;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IncludeTokenOnFailedValidation = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(discordOptions.Value.JwtSecret)),
                };
            });

        services.AddTransient<IClaimsTransformation, WebUserClaimsTransformer>();

        // Register command bus
        services.AddMediator(opt => { opt.ServiceLifetime = ServiceLifetime.Transient; });
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
        app.UseSerilogRequestLogging(o =>
        {
            o.MessageTemplate =
                "HTTP [{ClientIp}] {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            o.IncludeQueryInRequestPath = true;
        });

        app.UseRouting();
        app.UseCors(opt =>
        {
            opt.SetIsOriginAllowedToAllowWildcardSubdomains();
            opt.AllowAnyHeader();
            opt.AllowAnyMethod();
            opt.WithOrigins(Configuration.GetValue<string>("AllowedOrigins")?.Split(",") ?? ["*"])
                .AllowCredentials();
        });

        app.UseHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        app.UseAuthentication();
        app.UseAuthorization();
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
