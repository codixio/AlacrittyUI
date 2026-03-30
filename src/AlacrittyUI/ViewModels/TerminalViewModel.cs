using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.ViewModels;

public partial class TerminalViewModel : ObservableObject
{
    // Scrolling
    [ObservableProperty] private int _scrollingHistory = 10000;
    [ObservableProperty] private int _scrollingMultiplier = 3;

    // Shell
    [ObservableProperty] private string _shellProgram = string.Empty;
    [ObservableProperty] private string _shellArgs = string.Empty;

    // Selection
    [ObservableProperty] private bool _saveToClipboard;
    [ObservableProperty] private string _semanticEscapeChars = ",│`|:\"' ()[]{}<>\\t";

    // Mouse
    [ObservableProperty] private bool _hideMouseWhenTyping;

    // General
    [ObservableProperty] private bool _liveConfigReload = true;
    [ObservableProperty] private bool _ipcSocket = true;
    [ObservableProperty] private string _osc52 = "OnlyCopy";

    // Bell
    [ObservableProperty] private string _bellAnimation = "Linear";
    [ObservableProperty] private int _bellDuration;
    [ObservableProperty] private string _bellColor = "#ffffff";
    [ObservableProperty] private string _bellCommand = string.Empty;

    // General (extra)
    [ObservableProperty] private string _workingDirectory = string.Empty;
    [ObservableProperty] private string _importPaths = string.Empty;

    public string[] Osc52Options => TerminalConfig.Osc52Options;
    public string[] BellAnimationOptions => TerminalConfig.BellAnimationOptions;

    public void LoadFrom(TerminalConfig t)
    {
        ScrollingHistory = t.ScrollingHistory;
        ScrollingMultiplier = t.ScrollingMultiplier;
        ShellProgram = t.ShellProgram;
        ShellArgs = t.ShellArgs;
        SaveToClipboard = t.SaveToClipboard;
        SemanticEscapeChars = t.SemanticEscapeChars;
        HideMouseWhenTyping = t.HideMouseWhenTyping;
        LiveConfigReload = t.LiveConfigReload;
        IpcSocket = t.IpcSocket;
        Osc52 = t.Osc52;
        BellAnimation = t.BellAnimation;
        BellDuration = t.BellDuration;
        BellColor = t.BellColor;
        BellCommand = t.BellCommand;
        WorkingDirectory = t.WorkingDirectory;
        ImportPaths = string.Join("\n", t.Import);
    }

    public void ApplyTo(TerminalConfig t)
    {
        t.ScrollingHistory = ScrollingHistory;
        t.ScrollingMultiplier = ScrollingMultiplier;
        t.ShellProgram = ShellProgram;
        t.ShellArgs = ShellArgs;
        t.SaveToClipboard = SaveToClipboard;
        t.SemanticEscapeChars = SemanticEscapeChars;
        t.HideMouseWhenTyping = HideMouseWhenTyping;
        t.LiveConfigReload = LiveConfigReload;
        t.IpcSocket = IpcSocket;
        t.Osc52 = Osc52;
        t.BellAnimation = BellAnimation;
        t.BellDuration = BellDuration;
        t.BellColor = BellColor;
        t.BellCommand = BellCommand;
        t.WorkingDirectory = WorkingDirectory;
        t.Import = ImportPaths.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}
