using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.ViewModels;

public partial class WindowViewModel : ObservableObject
{
    [ObservableProperty] private int _columns;
    [ObservableProperty] private int _lines;
    [ObservableProperty] private int _paddingX;
    [ObservableProperty] private int _paddingY;
    [ObservableProperty] private bool _dynamicPadding;
    [ObservableProperty] private double _opacity = 1.0;
    [ObservableProperty] private bool _blur;
    [ObservableProperty] private string _decorations = "Full";
    [ObservableProperty] private string _startupMode = "Windowed";
    [ObservableProperty] private string _title = "Alacritty";
    [ObservableProperty] private bool _dynamicTitle = true;
    [ObservableProperty] private bool _resizeIncrements;
    [ObservableProperty] private int _positionX;
    [ObservableProperty] private int _positionY;
    [ObservableProperty] private bool _hasPosition;
    [ObservableProperty] private string _classInstance = "Alacritty";
    [ObservableProperty] private string _classGeneral = "Alacritty";
    [ObservableProperty] private string _decorationsThemeVariant = "None";
    [ObservableProperty] private string _optionAsAlt = "None";
    [ObservableProperty] private string _level = "Normal";

    public string[] DecorationOptions => WindowConfig.DecorationOptions;
    public string[] StartupModeOptions => WindowConfig.StartupModeOptions;
    public string[] ThemeVariantOptions => WindowConfig.ThemeVariantOptions;
    public string[] OptionAsAltOptions => WindowConfig.OptionAsAltOptions;
    public string[] LevelOptions => WindowConfig.LevelOptions;

    public void LoadFrom(WindowConfig w)
    {
        Columns = w.Columns;
        Lines = w.Lines;
        PaddingX = w.PaddingX;
        PaddingY = w.PaddingY;
        DynamicPadding = w.DynamicPadding;
        Opacity = w.Opacity;
        Blur = w.Blur;
        Decorations = w.Decorations;
        StartupMode = w.StartupMode;
        Title = w.Title;
        DynamicTitle = w.DynamicTitle;
        ResizeIncrements = w.ResizeIncrements;
        PositionX = w.PositionX;
        PositionY = w.PositionY;
        HasPosition = w.HasPosition;
        ClassInstance = w.ClassInstance;
        ClassGeneral = w.ClassGeneral;
        DecorationsThemeVariant = w.DecorationsThemeVariant;
        OptionAsAlt = w.OptionAsAlt;
        Level = w.Level;
    }

    public void ApplyTo(WindowConfig w)
    {
        w.Columns = Columns;
        w.Lines = Lines;
        w.PaddingX = PaddingX;
        w.PaddingY = PaddingY;
        w.DynamicPadding = DynamicPadding;
        w.Opacity = Opacity;
        w.Blur = Blur;
        w.Decorations = Decorations;
        w.StartupMode = StartupMode;
        w.Title = Title;
        w.DynamicTitle = DynamicTitle;
        w.ResizeIncrements = ResizeIncrements;
        w.PositionX = PositionX;
        w.PositionY = PositionY;
        w.HasPosition = HasPosition;
        w.ClassInstance = ClassInstance;
        w.ClassGeneral = ClassGeneral;
        w.DecorationsThemeVariant = DecorationsThemeVariant;
        w.OptionAsAlt = OptionAsAlt;
        w.Level = Level;
    }
}
