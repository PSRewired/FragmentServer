using System.Text;

namespace Fragment.NetSlum.Core.Extensions;

public static class DataExtensions
{
    public static byte[] StringToByteArray(this string hex) {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.ToUpper().Substring(x, 2), 16))
            .ToArray();
    }

    public static string ToHexString(this Span<byte> ba)
    {
        return ToHexString(ba.ToArray());
    }

    public static string ToHexString(this byte[] ba)
    {
        return BitConverter.ToString(ba).Replace("-","");
    }

    public static T[] Randomize<T>(this T[] arr)
    {
        var random = new Random();

        return arr.OrderBy(x => random.Next()).ToArray();
    }

    public static byte[] Write(this byte[] buffer, byte[] item)
    {
        var tempBuff = new byte[buffer.Length + item.Length];
        Buffer.BlockCopy(buffer, 0, tempBuff, 0, buffer.Length);
        Buffer.BlockCopy(item, 0, tempBuff, buffer.Length, item.Length);

        return tempBuff;
    }

    public static string ToHexDump(this byte[] bytes, int bytesPerLine = 16)
    {
        return ToHexDump(bytes.AsSpan(), bytesPerLine);
    }

    public static string ToHexDump(this Memory<byte> bytes, int bytesPerLine = 16)
    {
        return ToHexDump(bytes.Span, bytesPerLine);
    }

    public static string ToHexDump(this ReadOnlySpan<byte> bytes, int bytesPerLine = 16)
    {
        if (bytes == null)
        {
            return "<null>";
        }

        var bytesLength = bytes.Length;

        var HexChars = "0123456789ABCDEF".ToCharArray();

        var firstHexColumn =
            8                   // 8 characters for the address
            + 3;                  // 3 spaces

        var firstCharColumn = firstHexColumn
                              + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                              + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                              + 2;                  // 2 spaces

        var lineLength = firstCharColumn
                         + bytesPerLine + 2;           // - characters to show the ascii value

        var line = (new string(' ', lineLength - 2)).ToCharArray();
        var expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
        var result = new StringBuilder(expectedLines * lineLength);

        for (var i = 0; i < bytesLength; i += bytesPerLine)
        {
            line[0] = HexChars[(i >> 28) & 0xF];
            line[1] = HexChars[(i >> 24) & 0xF];
            line[2] = HexChars[(i >> 20) & 0xF];
            line[3] = HexChars[(i >> 16) & 0xF];
            line[4] = HexChars[(i >> 12) & 0xF];
            line[5] = HexChars[(i >> 8) & 0xF];
            line[6] = HexChars[(i >> 4) & 0xF];
            line[7] = HexChars[(i >> 0) & 0xF];

            var hexColumn = firstHexColumn;
            var charColumn = firstCharColumn;

            for (var j = 0; j < bytesPerLine; j++)
            {
                if (j > 0 && (j & 7) == 0)
                {
                    hexColumn++;
                }

                if (i + j >= bytesLength)
                {
                    line[hexColumn] = ' ';
                    line[hexColumn + 1] = ' ';
                    line[charColumn] = ' ';
                }
                else
                {
                    var b = bytes[i + j];
                    line[hexColumn] = HexChars[(b >> 4) & 0xF];
                    line[hexColumn + 1] = HexChars[b & 0xF];
                    line[charColumn] = asciiSymbol( b );
                }
                hexColumn += 3;
                charColumn++;
            }

            result.AppendLine(new string(line));
        }
        return result.ToString();
    }

    static char asciiSymbol( byte val )
    {
        if( val < 32 )
        {
            return '.';  // Non-printable ASCII
        }

        if( val < 127 )
        {
            return (char)val;   // Normal ASCII
        }

        // Handle the hole in Latin-1
        if( val == 127 )
        {
            return '.';
        }

        if( val < 0x90 )
        {
            return "€.‚ƒ„…†‡ˆ‰Š‹Œ.Ž."[ val & 0xF ];
        }

        if( val < 0xA0 )
        {
            return ".‘’“”•–—˜™š›œ.žŸ"[ val & 0xF ];
        }

        if( val == 0xAD )
        {
            return '.';   // Soft hyphen: this symbol is zero-width even in monospace fonts
        }

        return (char)val;   // Normal Latin-1
    }
}
