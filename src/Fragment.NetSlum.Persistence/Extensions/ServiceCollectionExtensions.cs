using Fragment.NetSlum.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fragment.NetSlum.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds automatic execution of EF context migrations at application boot so long as DOTNET_EF_AUTO_MIGRATE is not falsy.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <typeparam name="TContext">The <see cref="DbContext" /> used to analyze and execute pending migrations</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddAutoMigrations<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddHostedService<AutoDbContextMigrationService<TContext>>();

        return services;
    }
}
