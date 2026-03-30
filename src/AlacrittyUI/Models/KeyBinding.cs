namespace AlacrittyUI.Models;

public class KeyBinding
{
    public string Key { get; set; } = string.Empty;
    public string Mods { get; set; } = string.Empty;
    public string Mode { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public List<string> CommandArgs { get; set; } = [];
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

    public static readonly string[] CommonKeys = [
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
        "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
        "Key0", "Key1", "Key2", "Key3", "Key4", "Key5", "Key6", "Key7", "Key8", "Key9",
        "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
        "F13", "F14", "F15", "F16", "F17", "F18", "F19", "F20", "F21", "F22", "F23", "F24",
        "Return", "Space", "Tab", "Back", "Delete", "Insert",
        "Home", "End", "PageUp", "PageDown",
        "Up", "Down", "Left", "Right",
        "Escape", "Grave", "Minus", "Equals",
        "LBracket", "RBracket", "Semicolon", "Apostrophe",
        "Comma", "Period", "Slash", "Backslash",
        "NumpadEnter", "Numpad0", "Numpad1", "Numpad2", "Numpad3",
        "Numpad4", "Numpad5", "Numpad6", "Numpad7", "Numpad8", "Numpad9",
        "NumpadAdd", "NumpadSubtract", "NumpadMultiply", "NumpadDivide", "NumpadDecimal"
    ];

    public static readonly string[] ModeOptions = ["", "~Vi", "Vi", "~Search", "Search", "~Vi|~Search"];
}

public class KeyboardConfig
{
    public List<KeyBinding> Bindings { get; set; } = [];
}
