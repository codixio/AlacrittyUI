using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using AlacrittyUI.Resources;
using AlacrittyUI.Services;
using AlacrittyUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AlacrittyUI.Views;

public partial class MainWindow : Window
{
    private readonly AppSettingsService _appSettings;
    private bool _forceClose;

    public MainWindow()
    {
        InitializeComponent();

        _appSettings = App.Services.GetRequiredService<AppSettingsService>();
        ApplySettings();

        Closing += OnClosing;
    }

    private void ApplySettings()
    {
        var s = _appSettings.Settings;
        Width = s.WindowWidth;
        Height = s.WindowHeight;

        if (s.WindowX.HasValue && s.WindowY.HasValue)
            Position = new PixelPoint((int)s.WindowX.Value, (int)s.WindowY.Value);
        else
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        const double scaleEpsilon = 0.01;
        if (Math.Abs(s.UiScale - AppSettings.DefaultUiScale) > scaleEpsilon)
            RenderTransform = new ScaleTransform(s.UiScale, s.UiScale);
    }

    private void SaveWindowState()
    {
        var s = _appSettings.Settings;
        s.WindowWidth = Width;
        s.WindowHeight = Height;
        s.WindowX = Position.X;
        s.WindowY = Position.Y;

        if (DataContext is MainWindowViewModel vm && !string.IsNullOrEmpty(vm.ConfigPath))
            s.LastConfigPath = vm.ConfigPath;

        _appSettings.Save();
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        SaveWindowState();

        if (_forceClose) return;

        if (DataContext is MainWindowViewModel { IsDirty: true } vm)
        {
            e.Cancel = true;

            // show unsaved dialog — on save or discard, close the window
            vm.GuardUnsavedChanges(() =>
            {
                _forceClose = true;
                Close();
            });
        }
    }

    private async void OnBrowseClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is MainWindowViewModel vm)
            {
                // guard unsaved changes before loading a new file
                if (vm.IsDirty)
                {
                    vm.GuardUnsavedChanges(() => DoBrowse());
                    return;
                }
            }

            await DoBrowseAsync();
        }
        catch (Exception ex)
        {
            Log.ForContext<MainWindow>().Error(ex, "Failed to open file picker");
        }
    }

    private async void DoBrowse()
    {
        try
        {
            await DoBrowseAsync();
        }
        catch (Exception ex)
        {
            Log.ForContext<MainWindow>().Error(ex, "Failed to open file picker");
        }
    }

    private async Task DoBrowseAsync()
    {
        try
        {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = Strings.DialogOpenConfig,
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("TOML") { Patterns = ["*.toml"] },
                new FilePickerFileType("All Files") { Patterns = ["*"] }
            ]
        });

        if (files.Count > 0 && DataContext is MainWindowViewModel vm)
        {
            var path = files[0].TryGetLocalPath();
            if (path != null)
                vm.LoadConfigFromPath(path);
        }
        }
        catch (Exception ex)
        {
            Log.ForContext<MainWindow>().Error(ex, "File picker failed");
        }
    }

    private void OnConfigPathClick(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
            vm.OpenConfigInEditorCommand.Execute(null);
    }
}
