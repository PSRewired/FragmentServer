using System.Text;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Request;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Extensions;

public static class PacketExtensions
{
    /// <summary>
    /// Read an array of bytes until a string until a null terminator is encountered.
    /// If no null terminator is found, the array is assumed to be the entire string content
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetNullTerminatedString(this byte[] data)
    {
        var inx = Array.FindIndex(data, 0, (x) => x == 0);//search for 0

        return inx >= 0 ? Encoding.UTF8.GetString(data, 0, inx) : Encoding.UTF8.GetString(data);
    }

    /// <summary>
    /// Replaces bytes in an existing packet with new data specified.
    /// </summary>
    /// <param name="sourceData"></param>
    /// <param name="replaceValue"></param>
    /// <param name="offset"></param>
    public static byte[] Replace(this byte[] sourceData, byte[] replaceValue, int offset = 0)
    {
        Buffer.BlockCopy(replaceValue, 0, sourceData, offset, sourceData.Length < replaceValue.Length ? sourceData.Length : replaceValue.Length);

        return sourceData;
    }

    public static Span<byte> Replace(this Span<byte> sourceData, Span<byte> replaceValue, int offset = 0)
    {
        var replaceOffset = Math.Min(sourceData.Length, replaceValue.Length);
        replaceValue[..replaceOffset].CopyTo(sourceData[offset..]);

        return sourceData;
    }

    public static Memory<byte> Replace(this Memory<byte> sourceData, Memory<byte> replaceValue, int offset = 0)
    {
        var replaceOffset = Math.Min(sourceData.Length, replaceValue.Length);
        replaceValue[..replaceOffset].CopyTo(sourceData[offset..]);

        return sourceData;
    }

    /// <summary>
    /// Casts different TcpSession types into callable response generators based on BaseRequest implementations
    /// </summary>
    /// <param name="ro"></param>
    /// <param name="session"></param>
    /// <param name="request"></param>
    /// <typeparam name="TSession"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static async Task<ICollection<FragmentMessage>> CreateResponse<TSession>(this BaseRequest ro, TSession session, FragmentMessage request)
    {
        return session switch
        {
            FragmentTcpSession tcpSession => await ro.GetResponse(tcpSession, request),
            _ => throw new ArgumentException($"{session?.GetType()} is not a valid session type")
        };
    }
}
