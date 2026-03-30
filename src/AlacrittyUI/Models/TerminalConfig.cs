namespace AlacrittyUI.Models;

public class TerminalConfig
{
    // scrolling
    public int ScrollingHistory { get; set; } = 10000;
    public int ScrollingMultiplier { get; set; } = 3;

    // shell
    public string ShellProgram { get; set; } = string.Empty;
    public string ShellArgs { get; set; } = string.Empty;

    // selection
    public bool SaveToClipboard { get; set; }
    public string SemanticEscapeChars { get; set; } = ",│`|:\"' ()[]{}<>\\t";

    // mouse
    public bool HideMouseWhenTyping { get; set; }

    // general
    public bool LiveConfigReload { get; set; } = true;
    public bool IpcSocket { get; set; } = true;
    public string Osc52 { get; set; } = "OnlyCopy";
    public string WorkingDirectory { get; set; } = string.Empty;
    public List<string> Import { get; set; } = [];

    // bell
    public string BellAnimation { get; set; } = "Linear";
    public int BellDuration { get; set; }
    public string BellColor { get; set; } = "#ffffff";
    public string BellCommand { get; set; } = string.Empty;

    public static readonly string[] Osc52Options = ["Disabled", "OnlyCopy", "OnlyPaste", "CopyPaste"];
    public static readonly string[] BellAnimationOptions = ["Ease", "EaseOut", "EaseOutSine", "EaseOutQuad", "EaseOutCubic", "EaseOutQuart", "EaseOutQuint", "EaseOutExpo", "EaseOutCirc", "Linear"];
}
