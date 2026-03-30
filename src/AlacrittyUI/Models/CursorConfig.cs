namespace AlacrittyUI.Models;

public class CursorConfig
{
    public string Shape { get; set; } = "Block";
    public string Blinking { get; set; } = "Off";
    public int BlinkInterval { get; set; } = 750;
    public int BlinkTimeout { get; set; } = 5;
    public bool UnfocusedHollow { get; set; } = true;
    public double Thickness { get; set; } = 0.15;
    // vi_mode_style can be "None" or a {shape, blinking} table
    public bool ViModeEnabled { get; set; }
    public string ViModeShape { get; set; } = "Block";
    public string ViModeBlinking { get; set; } = "Off";

    public static readonly string[] ShapeOptions = ["Block", "Underline", "Beam"];
    public static readonly string[] BlinkingOptions = ["Never", "Off", "On", "Always"];
}
