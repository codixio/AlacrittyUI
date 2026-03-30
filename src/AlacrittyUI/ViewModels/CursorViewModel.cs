using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.ViewModels;

public partial class CursorViewModel : ObservableObject
{
    [ObservableProperty] private string _shape = "Block";
    [ObservableProperty] private string _blinking = "Off";
    [ObservableProperty] private int _blinkInterval = 750;
    [ObservableProperty] private int _blinkTimeout = 5;
    [ObservableProperty] private bool _unfocusedHollow = true;
    [ObservableProperty] private double _thickness = 0.15;
    [ObservableProperty] private bool _viModeEnabled;
    [ObservableProperty] private string _viModeShape = "Block";
    [ObservableProperty] private string _viModeBlinking = "Off";

    public string[] ShapeOptions => CursorConfig.ShapeOptions;
    public string[] BlinkingOptions => CursorConfig.BlinkingOptions;

    public void LoadFrom(CursorConfig c)
    {
        Shape = c.Shape;
        Blinking = c.Blinking;
        BlinkInterval = c.BlinkInterval;
        BlinkTimeout = c.BlinkTimeout;
        UnfocusedHollow = c.UnfocusedHollow;
        Thickness = c.Thickness;
        ViModeEnabled = c.ViModeEnabled;
        ViModeShape = c.ViModeShape;
        ViModeBlinking = c.ViModeBlinking;
    }

    public void ApplyTo(CursorConfig c)
    {
        c.Shape = Shape;
        c.Blinking = Blinking;
        c.BlinkInterval = BlinkInterval;
        c.BlinkTimeout = BlinkTimeout;
        c.UnfocusedHollow = UnfocusedHollow;
        c.Thickness = Thickness;
        c.ViModeEnabled = ViModeEnabled;
        c.ViModeShape = ViModeShape;
        c.ViModeBlinking = ViModeBlinking;
    }
}
