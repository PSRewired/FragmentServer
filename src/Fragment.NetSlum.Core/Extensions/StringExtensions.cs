using System.Text;

namespace Fragment.NetSlum.Core.Extensions;

public static class StringExtensions
{
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
    /// Transforms a utf8 encoded string to a Shift-JIS encoded byte array
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Span<byte> ToShiftJis(this string text)
    {
        return new Span<byte>(ShiftJisEncoder.GetBytes(text));
    }

    /// <summary>
    /// Read an array of bytes until a string until a null terminator is encountered.
    /// If no null terminator is found, the array is assumed to be the entire string content
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetNullTerminatedString(this Span<byte> data)
    {
        data = ReadToNullByte(data);

        return Encoding.UTF8.GetString(data);
    }

    private static Span<byte> ReadToNullByte(this Span<byte> arr)
    {
        var nullIndex = 0;

        while (nullIndex < arr.Length - 1)
        {
            if (arr[nullIndex] == 0)
            {
                break;
            }

            nullIndex++;
        }

        return arr[..nullIndex];
    }
}
