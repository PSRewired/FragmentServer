using Fragment.NetSlum.Networking.Sessions;
using Serilog;

namespace Fragment.NetSlum.Networking.Packets.Request;

public abstract class BaseRequest
{
    public ILogger Log => Serilog.Log.ForContext(GetType());

    public virtual Task<byte[]> GetResponse(FragmentTcpSession session, byte[] request)
    {
        throw new NotImplementedException($"Class [{GetType().Name}] does not support this method.");
    }
}
