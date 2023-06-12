using System;
using System.Linq;
using Fragment.NetSlum.Core.CommandBus;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Fragment.NetSlum.Core.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the command bus infrastructure to the service container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types">A type that exists inside of the project/assembly that contains handlers that can be registered</param>
    /// <returns></returns>
    public static IServiceCollection AddCommandBus(this IServiceCollection services, params Type[] types)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(types.Select(t => t.Assembly).ToArray());
            cfg.TypeEvaluator = t =>
            {
                Log.Information(t.Name);
                return true;
            };
        });

        services.AddScoped<ICommandBus, MediatorCommandBus>();

        return services;
    }
}
