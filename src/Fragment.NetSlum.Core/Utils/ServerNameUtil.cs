using System.Text.RegularExpressions;

namespace Fragment.NetSlum.Core.Utils;

public static partial class ServerNameUtil
{
    [GeneratedRegex(@"^(.*)\|(.*)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture, 1000)]
    public static partial Regex CategorySeparatorRegex();

    public static string FormatServerName(string serverName)
    {
        var match = CategorySeparatorRegex().Match(serverName);

        return match.Success ? match.Groups[2].Value : serverName;
    }
}
