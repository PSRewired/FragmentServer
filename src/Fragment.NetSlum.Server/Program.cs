using System;
using System.Data;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Swagger;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using ZiggyCreatures.Caching.Fusion;

var builder = WebApplication.CreateSlimBuilder(args);

builder.WebHost
    .UseKestrelCore()
    .ConfigureKestrel(opt =>
    {
        opt.AddServerHeader = false;
    })
    .ConfigureAppConfiguration((_, config) =>
    {
        var assemblyConfigurationAttribute =
            typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
        var buildConfigurationName = assemblyConfigurationAttribute?.Configuration;

        config.AddJsonFile("serverConfig.json", false, true);
        config.AddJsonFile("serverConfig.Local.json", true, true);

        if (buildConfigurationName == "Release")
        {
            config.AddJsonFile("serverConfig.Production.json", true, true);
        }

        config.AddEnvironmentVariables()
            .AddCommandLine(args);
    })
    .UseKestrelHttpsConfiguration();

builder.Host
    .UseSerilog((context, _, configuration) => configuration.ReadFrom
            .Configuration(context.Configuration)
        , writeToProviders: true)
    .ConfigureLogging((_, logger) =>
    {
        logger.ClearProviders();
        logger.AddEventSourceLogger();
    });

var connectionString = builder.Configuration.GetConnectionString("Database");

// Add failsafe to ensure the database is never executed on the original one
if (connectionString!.Contains("Database=fragment;"))
{
    throw new ConstraintException(
        "Auto-migrations have been disabled. The connection string contains the old database information!");
}

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.ShortSchemaNames = true;
        o.EnableJWTBearerAuth = false;
        o.MaxEndpointVersion = 1;
        o.DocumentSettings = d =>
        {
            d.Version = "v1";
            d.Title = "Fragment.Netslum";
            d.Description = "REST API that provides information from the .hack//Fragment server";
        };
    });
builder.Services
    .Configure<JsonSerializerOptions>(opt =>
    {
        opt.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .AddRouting(opt => opt.LowercaseUrls = true)
    .AddAuthorization();

builder.Services
    .AddDbContext<FragmentContext>((provider, opt) =>
    {
        opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        opt.AddInterceptors(provider.GetRequiredService<EntityChangeInterceptor>());
    })
    .AddAutoMigrations<FragmentContext>();

builder.Services.UseEntityListener()
    .AddListener<TimestampableEntityListener>()
    .AddListener<CharacterStatsChangeListener>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<FragmentContext>()
    .AddPrivateMemoryHealthCheck(2147483648)
    .AddProcessAllocatedMemoryHealthCheck(2048)
    ;

builder.Services.AddMemoryCache();
builder.Services.AddFusionCache()
    .WithDefaultEntryOptions(opt =>
    {
        opt.Duration = TimeSpan.FromMinutes(5);
        opt.SkipBackplaneNotifications = true;
    })
    .WithMemoryBackplane(opt => { opt.ConnectionId = Assembly.GetEntryAssembly()!.GetName().Name; });

builder.Services.Configure<DiscordAuthOptions>(builder.Configuration.GetSection("Authentication"));

builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddTransient<IClaimsTransformation, WebUserClaimsTransformer>();

// Register command bus
builder.Services.AddMediator(opt => { opt.ServiceLifetime = ServiceLifetime.Transient; });
builder.Services.AddScoped<ICommandBus, MediatorCommandBus>();

builder.Services.AddCors();

builder.Services.AddSingleton<ImageConverter>();

builder.Services.AddPacketHandling();
builder.Services.Configure<ServerConfiguration>(builder.Configuration.GetSection("TcpServer"));
builder.Services.AddSingleton<ITcpServer, Server>();
builder.Services.AddSingleton(typeof(ChatLobbyStore));
builder.Services.AddHostedService<ServerBackgroundService>();
builder.Services.AddHostedService<ClientTickService>();
builder.Services.AddHostedService<ChatLobbyBackgroundService>();


var app = builder.Build();

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
    opt.WithOrigins(app.Configuration.GetValue<string>("AllowedOrigins")?.Split(",") ?? ["*"])
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseHealthChecks("/api/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});
app.UseFastEndpoints(opt =>
    {
        opt.Endpoints.ShortNames = true;
        opt.Endpoints.RoutePrefix = "api";
        opt.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.Serializer.Options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        opt.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
    })
    .UseSwaggerGen(opt => { opt.Path = "/api/_doc/{documentName}/swagger.json"; });

app.UseSwaggerUi(opt =>
{
    opt.Path = "/api/_doc";
    opt.DocumentPath = "/api/_doc/{documentName}/swagger.json";
    opt.CustomStylesheetPath = "/swagger/css/ui.css";
});

app.MapScalarApiReference("/api/_doc/scalar/", opt => { opt.OpenApiRoutePattern = "/api/_doc/{documentName}/swagger.json"; });

// Register additional encodings, since dotnet core only supports ASCII, ISO-8859, and UTF by default
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Log.ForContext<Program>().Warning("Server is live!");


app.Run();
