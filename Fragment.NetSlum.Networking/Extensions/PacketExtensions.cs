using Fragment.NetSlum.Networking.Packets.Request;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Extensions;

public static class PacketExtensions
{
    /// <summary>
    /// Casts different TcpSession types into callable response generators based on BaseRequest implementations
    /// </summary>
    /// <param name="ro"></param>
    /// <param name="session"></param>
    /// <param name="request"></param>
    /// <typeparam name="TSession"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<byte[]> CreateResponse<TSession>(this BaseRequest ro, TSession session, byte[] request)
    {
        return session switch
        {
            FragmentTcpSession tcpSession => await ro.GetResponse(tcpSession, request),
            _ => throw new ArgumentException($"{session.GetType()} is not a valid session type")
        };
    }
}
