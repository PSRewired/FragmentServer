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

    public EntityListenerBuilder AddListener<TEntityListener>()
    {
        Services.AddScoped(typeof(IEntityChangeListener), typeof(TEntityListener));

        return this;
    }
}
