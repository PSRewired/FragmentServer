using Fragment.NetSlum.Persistence.Builders;
using Fragment.NetSlum.Persistence.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Fragment.NetSlum.Persistence.Extensions;

public static class EntityListenerServerExtensions
{
    public static EntityListenerBuilder UseEntityListener(this IServiceCollection services)
    {
        services.AddScoped<EntityChangeInterceptor>();

        return new EntityListenerBuilder(services);
    }
}
