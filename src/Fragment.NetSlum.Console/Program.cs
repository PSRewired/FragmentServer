using System;
using System.Text;
using Fragment.NetSlum.Console.Commands;
using Fragment.NetSlum.Console.DependencyInjection;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Persistence.Interceptors;
using Fragment.NetSlum.Persistence.Listeners;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Serilog;

// Needed for Shift-JIS encoding
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddSerilog(dispose: true));

services.UseEntityListener()
    .AddListener<TimestampableEntityListener>()
    .AddListener<CharacterStatsChangeListener>();

services.AddDbContext<FragmentContext>((provider, opt) =>
{
    var connString = Environment.GetEnvironmentVariable("ConnectionStrings__Database");
    opt.UseMySql(connString, ServerVersion.AutoDetect(connString));
    opt.AddInterceptors(provider.GetRequiredService<EntityChangeInterceptor>());
});

services.AddDbContext<OldFragmentContext>(opt =>
{
    var connString = Environment.GetEnvironmentVariable("ConnectionStrings__OldDatabase");
    opt.UseMySql(connString, ServerVersion.AutoDetect(connString));
});

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.PropagateExceptions();
    config.AddCommand<MigratePlayersCommand>("migrate-players")
        .WithDescription("Migrates the old server player entities to the new format used by this server");
    config.AddCommand<MigrateGuildsCommand>("migrate-guilds")
        .WithDescription("Migrates the old server guild entities to the new format used by this server");
    config.AddCommand<MigrateGuildShopCommand>("migrate-guild-shop")
        .WithDescription("Migrates the old server guild shop items to the new format used by this server");
    config.AddCommand<MigrateBbsCommand>("migrate-bbs")
        .WithDescription("Migrates the old server BBS entities to the new format used by this server");
});

return app.Run(args);

