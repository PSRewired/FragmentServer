using Fragment.NetSlum.Networking.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fragment.NetSlum.Networking.Pipeline.Builder;

public static class PacketPipelineBuilderExtensions
{
    public static PacketPipelineBuilder AddPacketPipeline(this IServiceCollection services)
    {
        services.TryAddScoped<FragmentPacketHandler>();
        
        var builder = new PacketPipelineBuilder(services);

        return builder;
    }
}