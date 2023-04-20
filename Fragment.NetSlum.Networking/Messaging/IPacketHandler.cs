using Fragment.NetSlum.Core.DependencyInjection;

namespace Fragment.NetSlum.Networking.Messaging;

public interface IPacketHandler<in TRequest>
{
    public Task<byte[]> CreateResponse<TSession>(TSession session, TRequest o) where TSession : IScopeable;
}
