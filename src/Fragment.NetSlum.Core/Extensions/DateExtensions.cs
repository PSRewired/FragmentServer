using System;

namespace Fragment.NetSlum.Core.Extensions;

public static class DateExtensions
{
    private static DateTime epoch = new DateTime(1970, 1, 1);

    public static ulong ToEpoch(this DateTime dt)
    {
        return (ulong) dt.Subtract(epoch).TotalSeconds;
    }
}
