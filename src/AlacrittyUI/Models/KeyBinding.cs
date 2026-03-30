namespace AlacrittyUI.Models;

public class KeyBinding
{
    public string Key { get; set; } = string.Empty;
    public string Mods { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string Chars { get; set; } = string.Empty;

    public static readonly string[] ActionOptions =
    [
        "ReceiveChar", "None",
        "Paste", "Copy", "CopySelection", "PasteSelection",
        "IncreaseFontSize", "DecreaseFontSize", "ResetFontSize",
        "ScrollPageUp", "ScrollPageDown", "ScrollHalfPageUp", "ScrollHalfPageDown",
        "ScrollLineUp", "ScrollLineDown", "ScrollToTop", "ScrollToBottom",
        "ClearHistory", "Hide", "Minimize", "Quit",
        "ClearLogNotice", "SpawnNewInstance", "CreateNewWindow",
        "ToggleFullscreen", "ToggleMaximized", "ToggleSimpleFullscreen",
        "ClearSelection", "ToggleViMode",
        "SearchForward", "SearchBackward"
    ];

    public static readonly string[] ModOptions = ["Control", "Shift", "Alt", "Super", "Command", "Option"];
}

public class KeyboardConfig
{
    public List<KeyBinding> Bindings { get; set; } = [];
}
