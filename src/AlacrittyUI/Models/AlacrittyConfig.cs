using Tomlyn.Model;

namespace AlacrittyUI.Models;

public class AlacrittyConfig
{
    public ColorPalette Colors { get; set; } = new();
    public FontConfig Font { get; set; } = new();
    public WindowConfig Window { get; set; } = new();
    public CursorConfig Cursor { get; set; } = new();
    public TerminalConfig Terminal { get; set; } = new();
    public HintsConfig Hints { get; set; } = new();
    public KeyboardConfig Keyboard { get; set; } = new();
    public DebugConfig Debug { get; set; } = new();

    /// <summary>
    /// Raw TOML document preserved for pass-through of unknown sections.
    /// </summary>
    public TomlTable? RawDocument { get; set; }
}
