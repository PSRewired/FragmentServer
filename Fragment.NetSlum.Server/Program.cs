using System.Reflection;
using Fragment.NetSlum.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_, builder) =>
    {
        var assemblyConfigurationAttribute =
            typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
        var buildConfigurationName = assemblyConfigurationAttribute?.Configuration;

        builder.AddJsonFile("serverConfig.json", false, true);
        builder.AddJsonFile("serverConfig.Local.json", true, true);

        if (buildConfigurationName == "Release")
        {
            builder.AddJsonFile("serverConfig.Production.json", true, true);
        }

        builder.AddEnvironmentVariables()
            .AddCommandLine(args);
    })
    .ConfigureWebHostDefaults(builder =>
    {
        builder.UseStartup<Startup>();
    })
    .UseSerilog((hostingContext, _, loggerConfig) =>
    {
        loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
    });


var app = builder.Build();

await app.RunAsync();
