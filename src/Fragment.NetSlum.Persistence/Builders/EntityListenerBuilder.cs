using Fragment.NetSlum.Persistence.Listeners;
using Microsoft.Extensions.DependencyInjection;

namespace Fragment.NetSlum.Persistence.Builders;

public class EntityListenerBuilder
{
    public IServiceCollection Services { get; }

    public EntityListenerBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public EntityListenerBuilder AddListener<TEntityListener>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        Services.Add(new ServiceDescriptor(typeof(IEntityChangedListener), typeof(TEntityListener), lifetime));

        return this;
    }
}
