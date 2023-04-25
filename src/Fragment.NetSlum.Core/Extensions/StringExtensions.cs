using System.Text;

namespace Fragment.NetSlum.Core.Extensions;

public static class StringExtensions
{
    private const int ShiftJisEncodingId = 932;
    private static Encoding ShiftJisEncoder => Encoding.GetEncoding(ShiftJisEncodingId);

    public static string ToShiftJisString(this Span<byte> stringBytes)
    {
        return ShiftJisEncoder.GetString(ReadToNullByte(stringBytes));
    }

    public static Span<byte> ToShiftJis(this string utf8string)
    {
        return new Span<byte>(ShiftJisEncoder.GetBytes(utf8string));
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
