namespace AlacrittyUI.Models;

public class ColorPalette
{
    // Primary
    public string Foreground { get; set; } = "#d8d8d8";
    public string Background { get; set; } = "#181818";
    public string? DimForeground { get; set; } = "#828482";
    public string? BrightForeground { get; set; }

    // Normal
    public string NormalBlack { get; set; } = "#181818";
    public string NormalRed { get; set; } = "#ac4242";
    public string NormalGreen { get; set; } = "#90a959";
    public string NormalYellow { get; set; } = "#f4bf75";
    public string NormalBlue { get; set; } = "#6a9fb5";
    public string NormalMagenta { get; set; } = "#aa759f";
    public string NormalCyan { get; set; } = "#75b5aa";
    public string NormalWhite { get; set; } = "#d8d8d8";

    // Bright
    public string BrightBlack { get; set; } = "#6b6b6b";
    public string BrightRed { get; set; } = "#c55555";
    public string BrightGreen { get; set; } = "#aac474";
    public string BrightYellow { get; set; } = "#feca88";
    public string BrightBlue { get; set; } = "#82b8c8";
    public string BrightMagenta { get; set; } = "#c28cb8";
    public string BrightCyan { get; set; } = "#93d3c3";
    public string BrightWhite { get; set; } = "#f8f8f8";

    // Cursor
    public string? CursorText { get; set; }
    public string? CursorColor { get; set; }

    // Selection
    public string? SelectionText { get; set; }
    public string? SelectionBackground { get; set; }

    // Search
    public string? SearchMatchForeground { get; set; }
    public string? SearchMatchBackground { get; set; }
    public string? SearchFocusedForeground { get; set; }
    public string? SearchFocusedBackground { get; set; }

    // Footer bar
    public string? FooterBarForeground { get; set; }
    public string? FooterBarBackground { get; set; }

    // Vi mode cursor
    public string? ViModeCursorText { get; set; }
    public string? ViModeCursorColor { get; set; }

    // Hints
    public string? HintsStartForeground { get; set; }
    public string? HintsStartBackground { get; set; }
    public string? HintsEndForeground { get; set; }
    public string? HintsEndBackground { get; set; }

    // Line indicator
    public string? LineIndicatorForeground { get; set; }
    public string? LineIndicatorBackground { get; set; }

    // Dim (auto-calculated if absent)
    public string? DimBlack { get; set; }
    public string? DimRed { get; set; }
    public string? DimGreen { get; set; }
    public string? DimYellow { get; set; }
    public string? DimBlue { get; set; }
    public string? DimMagenta { get; set; }
    public string? DimCyan { get; set; }
    public string? DimWhite { get; set; }

    // Flags
    public bool DrawBoldTextWithBrightColors { get; set; }
    public bool TransparentBackgroundColors { get; set; }
}
