using System;
using Fragment.NetSlum.Console.Commands;
using Fragment.NetSlum.Console.DependencyInjection;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();

services.AddDbContext<FragmentContext>(opt =>
{
    var connString = Environment.GetEnvironmentVariable("ConnectionStrings_Database");
    opt.UseMySql(connString, ServerVersion.AutoDetect(connString));
});

services.AddDbContext<OldFragmentContext>(opt =>
{
    var connString = Environment.GetEnvironmentVariable("ConnectionStrings_OldDatabase");
    opt.UseMySql(connString, ServerVersion.AutoDetect(connString));
});

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.AddCommand<MigrateGuildsCommand>("migrate-guilds")
        .WithDescription("Migrates the old server guild entities to the new format used by this server");
});

return app.Run(args);

