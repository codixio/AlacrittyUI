namespace AlacrittyUI.Models;

public class FontConfig
{
    public double Size { get; set; } = 11.25;

    // normal font
    public string NormalFamily { get; set; } = "monospace";
    public string NormalStyle { get; set; } = "Regular";

    // bold font
    public string? BoldFamily { get; set; }
    public string BoldStyle { get; set; } = "Bold";

    // italic font
    public string? ItalicFamily { get; set; }
    public string ItalicStyle { get; set; } = "Italic";

    // bold italic font
    public string? BoldItalicFamily { get; set; }
    public string BoldItalicStyle { get; set; } = "Bold Italic";

    // offset (spacing between characters)
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }

    // glyph offset (individual glyph positioning)
    public int GlyphOffsetX { get; set; }
    public int GlyphOffsetY { get; set; }

    public bool BuiltinBoxDrawing { get; set; } = true;
}
