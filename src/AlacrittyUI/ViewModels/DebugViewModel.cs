using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.ViewModels;

public partial class DebugViewModel : ObservableObject
{
    [ObservableProperty] private bool _renderTimer;
    [ObservableProperty] private bool _persistentLogging;
    [ObservableProperty] private string _logLevel = "Warn";
    [ObservableProperty] private string _renderer = "None";
    [ObservableProperty] private bool _printEvents;
    [ObservableProperty] private bool _highlightDamage;
    [ObservableProperty] private bool _preferEgl;

    public string[] LogLevelOptions => DebugConfig.LogLevelOptions;
    public string[] RendererOptions => DebugConfig.RendererOptions;

    public void LoadFrom(DebugConfig d)
    {
        RenderTimer = d.RenderTimer;
        PersistentLogging = d.PersistentLogging;
        LogLevel = d.LogLevel;
        Renderer = d.Renderer;
        PrintEvents = d.PrintEvents;
        HighlightDamage = d.HighlightDamage;
        PreferEgl = d.PreferEgl;
    }

    public void ApplyTo(DebugConfig d)
    {
        d.RenderTimer = RenderTimer;
        d.PersistentLogging = PersistentLogging;
        d.LogLevel = LogLevel;
        d.Renderer = Renderer;
        d.PrintEvents = PrintEvents;
        d.HighlightDamage = HighlightDamage;
        d.PreferEgl = PreferEgl;
    }
}
