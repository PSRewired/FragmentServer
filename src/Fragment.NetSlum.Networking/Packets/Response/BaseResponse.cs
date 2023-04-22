using System.Runtime.CompilerServices;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Serilog;

namespace Fragment.NetSlum.Networking.Packets.Response;

public abstract class BaseResponse
{
    public ILogger Log => Serilog.Log.ForContext(GetType());

    public abstract FragmentMessage Build();

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public FragmentMessage Build(OpCodes type, byte[] payload)
    {
        return new FragmentMessage
        {
            Data = payload,
            OpCode = type,
        };
    }
}
