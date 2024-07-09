using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.DependencyInjection;

namespace Fragment.NetSlum.Networking.Messaging;

public interface IPacketHandler<TRequest>
{
    public ValueTask<ICollection<TRequest>> CreateResponse<TSession>(TSession session, TRequest o) where TSession : IScopeable;
}
