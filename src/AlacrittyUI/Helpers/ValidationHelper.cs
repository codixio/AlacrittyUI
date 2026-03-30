using System.Text.RegularExpressions;

namespace AlacrittyUI.Helpers;

public static partial class ValidationHelper
{
    [GeneratedRegex(@"^#[0-9a-fA-F]{6}$")]
    private static partial Regex HexColorRegex();

    public static bool IsValidHexColor(string? value)
        => !string.IsNullOrEmpty(value) && HexColorRegex().IsMatch(value);
}
