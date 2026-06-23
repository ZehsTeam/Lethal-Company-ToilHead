using System.Text.RegularExpressions;

namespace com.github.zehsteam.ToilHead.Extensions;

internal static class StringExtensions
{
    public static bool IsHexColor(this string value)
    {
        return Regex.IsMatch(value, @"^#?([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$");
    }
}
