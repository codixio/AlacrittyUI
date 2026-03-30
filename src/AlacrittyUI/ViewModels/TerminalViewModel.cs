using System.Collections.ObjectModel;
using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlacrittyUI.ViewModels;

public partial class ShellArgViewModel : ObservableObject
{
    [ObservableProperty] private string _value = string.Empty;
}

public partial class TerminalViewModel : ObservableObject
{
    // Scrolling
    [ObservableProperty] private int _scrollingHistory = 10000;
    [ObservableProperty] private int _scrollingMultiplier = 3;

    // Shell
    [ObservableProperty] private string _shellProgram = string.Empty;
    public ObservableCollection<ShellArgViewModel> ShellArgs { get; } = [];
    [ObservableProperty] private ShellArgViewModel? _selectedShellArg;

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
    private List<string> _bellCommandArgs = [];

    // General (extra)
    [ObservableProperty] private string _workingDirectory = string.Empty;
    [ObservableProperty] private string _importPaths = string.Empty;

    public string ShellProgramPlaceholder { get; } =
        OperatingSystem.IsWindows() ? "powershell.exe" : "/bin/bash";

    public string[] Osc52Options => TerminalConfig.Osc52Options;
    public string[] BellAnimationOptions => TerminalConfig.BellAnimationOptions;

    [RelayCommand]
    private void AddShellArg()
    {
        var arg = new ShellArgViewModel();
        ShellArgs.Add(arg);
        SelectedShellArg = arg;
    }

    [RelayCommand]
    private void RemoveShellArg()
    {
        if (SelectedShellArg == null) return;
        ShellArgs.Remove(SelectedShellArg);
        SelectedShellArg = null;
    }

    public void LoadFrom(TerminalConfig t)
    {
        ScrollingHistory = t.ScrollingHistory;
        ScrollingMultiplier = t.ScrollingMultiplier;
        ShellProgram = t.ShellProgram;
        ShellArgs.Clear();
        foreach (var arg in t.ShellArgs)
            ShellArgs.Add(new ShellArgViewModel { Value = arg });
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
        _bellCommandArgs = t.BellCommandArgs;
        WorkingDirectory = t.WorkingDirectory;
        ImportPaths = string.Join("\n", t.Import);
    }

    public void ApplyTo(TerminalConfig t)
    {
        t.ScrollingHistory = ScrollingHistory;
        t.ScrollingMultiplier = ScrollingMultiplier;
        t.ShellProgram = ShellProgram;
        t.ShellArgs = ShellArgs.Where(a => !string.IsNullOrWhiteSpace(a.Value)).Select(a => a.Value).ToList();
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
        t.BellCommandArgs = _bellCommandArgs;
        t.WorkingDirectory = WorkingDirectory;
        t.Import = ImportPaths.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}
