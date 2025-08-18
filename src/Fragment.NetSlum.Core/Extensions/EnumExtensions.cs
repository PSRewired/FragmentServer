using System;
using System.Collections.Generic;
using System.Linq;

namespace Fragment.NetSlum.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Casts a string into an enum. The string comparison performed is case-insensitive
    /// </summary>
    /// <param name="input">The string representation of an enum value</param>
    /// <typeparam name="TEnum">The enum type to cast to</typeparam>
    /// <returns></returns>
    public static TEnum ToEnum<TEnum>(this string input) where TEnum : struct, Enum
    {
        return Enum.Parse<TEnum>(input, true);
    }

    public static TEnum ToEnum<TEnum>(this ICollection<string> input) where TEnum : struct, Enum
    {
        return Enum.Parse<TEnum>(string.Join(',', input));
    }

    public static ICollection<string> ToStringCollection<TEnum>(this TEnum input) where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .Where(e => input.HasFlag(e))
            .Select(e => e.ToString())
            .ToList();
    }

    public static bool HasFlag<TEnum>(this TEnum? input, TEnum flag) where TEnum : struct, Enum
    {
        return input != null && input.Value.HasFlag(flag);
    }
}
