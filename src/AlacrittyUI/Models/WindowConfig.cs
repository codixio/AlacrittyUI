namespace AlacrittyUI.Models;

public class WindowConfig
{
    // dimensions (0 = auto)
    public int Columns { get; set; }
    public int Lines { get; set; }

    // padding
    public int PaddingX { get; set; }
    public int PaddingY { get; set; }
    public bool DynamicPadding { get; set; }

    // appearance
    public double Opacity { get; set; } = 1.0;
    public bool Blur { get; set; }
    public string Decorations { get; set; } = "Full";
    public string StartupMode { get; set; } = "Windowed";
    public string Title { get; set; } = "Alacritty";
    public bool DynamicTitle { get; set; } = true;
    public bool ResizeIncrements { get; set; }

    // position ("None" = not set)
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public bool HasPosition { get; set; }

    // window class (Linux/BSD only)
    public string ClassInstance { get; set; } = "Alacritty";
    public string ClassGeneral { get; set; } = "Alacritty";

    public string DecorationsThemeVariant { get; set; } = "None";
    public string OptionAsAlt { get; set; } = "None";
    public string Level { get; set; } = "Normal";

    public static readonly string[] DecorationOptions = ["Full", "None", "Transparent", "Buttonless"];
    public static readonly string[] StartupModeOptions = ["Windowed", "Maximized", "Fullscreen", "SimpleFullscreen"];
    public static readonly string[] ThemeVariantOptions = ["None", "Dark", "Light"];
    public static readonly string[] OptionAsAltOptions = ["None", "OnlyLeft", "OnlyRight", "Both"];
    public static readonly string[] LevelOptions = ["Normal", "AlwaysOnTop"];
}
