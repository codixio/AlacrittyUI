namespace AlacrittyUI.Models;

public class CursorConfig
{
    public const int DefaultBlinkInterval = 750;
    public const int DefaultBlinkTimeout = 5;
    public const double DefaultThickness = 0.15;

    public string Shape { get; set; } = "Block";
    public string Blinking { get; set; } = "Off";
    public int BlinkInterval { get; set; } = DefaultBlinkInterval;
    public int BlinkTimeout { get; set; } = DefaultBlinkTimeout;
    public bool UnfocusedHollow { get; set; } = true;
    public double Thickness { get; set; } = DefaultThickness;
    // vi_mode_style can be "None" or a {shape, blinking} table
    public bool ViModeEnabled { get; set; }
    public string ViModeShape { get; set; } = "Block";
    public string ViModeBlinking { get; set; } = "Off";

    public static readonly string[] ShapeOptions = ["Block", "Underline", "Beam"];
    public static readonly string[] BlinkingOptions = ["Never", "Off", "On", "Always"];
}
