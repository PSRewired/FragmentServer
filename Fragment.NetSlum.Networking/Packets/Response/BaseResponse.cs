using System.Runtime.CompilerServices;
using Fragment.NetSlum.Networking.Constants;
using Serilog;

namespace Fragment.NetSlum.Networking.Packets.Response;

public abstract class BaseResponse
{
    public ILogger Log => Serilog.Log.ForContext(GetType());

    public abstract byte[] Build();

    // NOTE: Using pooling here actually decreases performance because of the small buffers. Everything is Gen0'd
    // Pooled 29.92ms
    // newable: 26.87
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public byte[] Build(OpCodes type, byte packetClass, byte packetType, byte[] payload)
    {
        //TODO
        var buffer = new byte[payload.Length + 2];
        buffer[0] = packetClass;
        buffer[1] = packetType;

        Buffer.BlockCopy(payload, 0, buffer, 2, payload.Length);

        return Build(type, buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public byte[] Build(OpCodes type, byte[] payload)
    {
        var buffer = new byte[payload.Length + sizeof(ushort) + 1];
        buffer[0] = (byte) type;
        Buffer.BlockCopy(BitConverter.GetBytes((ushort) payload.Length), 0, buffer, 1, sizeof(ushort));
        Buffer.BlockCopy(payload, 0, buffer, sizeof(ushort) + 1, payload.Length);

        return buffer;
    }
}
