using System.Text;

namespace Fragment.NetSlum.Core.Extensions;

public static class StringExtensions
{
    // Fragment uses CP932 encoding. In C#, Shift-JIS is actually CP/MS932
    private const int ShiftJisEncodingId = 932;
    private static Encoding ShiftJisEncoder => Encoding.GetEncoding(ShiftJisEncodingId);

    /// <summary>
    /// Transforms a null-terminated Shift-JIS encoded byte array into its string representation
    /// </summary>
    /// <param name="stringBytes"></param>
    /// <returns></returns>
    public static string ToShiftJisString(this Span<byte> stringBytes)
    {
        return ShiftJisEncoder.GetString(stringBytes.ReadToNullByte());
    }

    /// <summary>
    /// Transforms a utf8 encoded string to a Shift-JIS encoded byte array. Appends a null-terminator by default
    /// </summary>
    /// <param name="text"></param>
    /// <param name="nullTerminated">Optionally append a null terminator to the end of the string</param>
    /// <returns></returns>
    public static Span<byte> ToShiftJis(this string text, bool nullTerminated = true)
    {
        var bytes = new Span<byte>(ShiftJisEncoder.GetBytes(text));

        if (bytes[^1] == 0 || !nullTerminated)
        {
            return bytes;
        }

        var withTerminator = new Span<byte>(new byte[bytes.Length + 1]);
        bytes.CopyTo(withTerminator);

        return withTerminator;

    }

    /// <summary>
    /// Read an array of bytes of a string until a null terminator is encountered.
    /// If no null terminator is found, the array is assumed to be the entire string content
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetNullTerminatedString(this Span<byte> data)
    {
        data = ReadToNullByte(data);

        return Encoding.UTF8.GetString(data);
    }

    /// <summary>
    /// Reads a string from binary data up until the null byte, without including it
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static Span<byte> ReadToNullByte(this Span<byte> arr)
    {
        var nullIndex = 0;

        do
        {
            if (arr[nullIndex] == 0)
            {
                break;
            }

            nullIndex++;
        }
        while (nullIndex < arr.Length) ;

        return arr[..nullIndex];
    }
}
