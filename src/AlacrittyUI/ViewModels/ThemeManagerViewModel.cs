using System.Collections.ObjectModel;
using AlacrittyUI.Models;
using AlacrittyUI.Resources;
using AlacrittyUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;

namespace AlacrittyUI.ViewModels;

public partial class ThemeManagerViewModel : ObservableObject
{
    private static readonly ILogger Logger = Log.ForContext<ThemeManagerViewModel>();

    private readonly ThemeService _themeService;
    private readonly MainWindowViewModel _mainVm;

    public ObservableCollection<ThemeInfo> BuiltInThemes { get; } = [];
    public ObservableCollection<ThemeInfo> UserThemes { get; } = [];

    [ObservableProperty]
    private ThemeInfo? _selectedTheme;

    [ObservableProperty]
    private string _newThemeName = string.Empty;

    public ThemeManagerViewModel(ThemeService themeService, MainWindowViewModel mainVm)
    {
        _themeService = themeService;
        _mainVm = mainVm;
        LoadThemes();
    }

    public void LoadThemes()
    {
        BuiltInThemes.Clear();
        foreach (var theme in _themeService.GetBuiltInThemes())
            BuiltInThemes.Add(theme);

        UserThemes.Clear();
        foreach (var theme in _themeService.GetUserThemes())
            UserThemes.Add(theme);
    }

    [RelayCommand]
    private void ApplyTheme()
    {
        if (SelectedTheme == null) return;

        try
        {
            var palette = _themeService.LoadPalette(SelectedTheme);
            _mainVm.ApplyPalette(palette);
            _mainVm.StatusText = Strings.StatusThemeApplied;
            Logger.Information("Applied theme {Name}", SelectedTheme.Name);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to apply theme {Name}", SelectedTheme.Name);
            _mainVm.StatusText = Strings.StatusThemeError;
        }
    }

    [RelayCommand]
    private void SaveCurrentAsTheme()
    {
        if (string.IsNullOrWhiteSpace(NewThemeName)) return;

        try
        {
            var palette = _mainVm.GetCurrentPalette();
            if (palette == null) return;

            _themeService.SaveTheme(NewThemeName.Trim(), palette);
            NewThemeName = string.Empty;
            LoadThemes();
            Logger.Information("Saved current colors as theme");
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to save theme");
            _mainVm.StatusText = Strings.StatusThemeError;
        }
    }

    [RelayCommand]
    private void DeleteTheme()
    {
        if (SelectedTheme is not { IsBuiltIn: false }) return;

        try
        {
            _themeService.DeleteUserTheme(SelectedTheme);
            LoadThemes();
            SelectedTheme = null;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to delete theme");
            _mainVm.StatusText = Strings.StatusThemeError;
        }
    }

    public void ImportFromPath(string path)
    {
        try
        {
            var palette = _themeService.ImportTheme(path);
            _mainVm.ApplyPalette(palette);
            _mainVm.StatusText = Strings.StatusThemeApplied;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to import theme from {Path}", path);
            _mainVm.StatusText = Strings.StatusThemeError;
        }
    }

    public void ExportToPath(string path)
    {
        try
        {
            var palette = _mainVm.GetCurrentPalette();
            if (palette == null) return;

            _themeService.ExportTheme(path, palette);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to export theme to {Path}", path);
            _mainVm.StatusText = Strings.StatusThemeError;
        }
    }
}
