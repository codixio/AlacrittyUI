using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using AlacrittyUI.ViewModels;
using Serilog;

namespace AlacrittyUI.Views;

public partial class TerminalView : UserControl
{
    public TerminalView()
    {
        InitializeComponent();
    }

    private async void OnBrowseShellClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Shell Program",
                AllowMultiple = false,
                FileTypeFilter = OperatingSystem.IsWindows()
                    ? [new FilePickerFileType("Executables") { Patterns = ["*.exe", "*.cmd", "*.bat"] },
                       new FilePickerFileType("All Files") { Patterns = ["*"] }]
                    : [new FilePickerFileType("All Files") { Patterns = ["*"] }]
            });

            if (files.Count > 0 && DataContext is TerminalViewModel vm)
            {
                var path = files[0].TryGetLocalPath();
                if (path != null)
                    vm.ShellProgram = path;
            }
        }
        catch (Exception ex)
        {
            Log.ForContext<TerminalView>().Error(ex, "Failed to open file picker for shell program");
        }
    }

    private async void OnBrowseWorkingDirClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Select Working Directory",
                AllowMultiple = false
            });

            if (folders.Count > 0 && DataContext is TerminalViewModel vm)
            {
                var path = folders[0].TryGetLocalPath();
                if (path != null)
                    vm.WorkingDirectory = path;
            }
        }
        catch (Exception ex)
        {
            Log.ForContext<TerminalView>().Error(ex, "Failed to open folder picker for working directory");
        }
    }

    private async void OnBrowseBellCommandClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Bell Command",
                AllowMultiple = false,
                FileTypeFilter = OperatingSystem.IsWindows()
                    ? [new FilePickerFileType("Executables") { Patterns = ["*.exe", "*.cmd", "*.bat"] },
                       new FilePickerFileType("All Files") { Patterns = ["*"] }]
                    : [new FilePickerFileType("All Files") { Patterns = ["*"] }]
            });

            if (files.Count > 0 && DataContext is TerminalViewModel vm)
            {
                var path = files[0].TryGetLocalPath();
                if (path != null)
                    vm.BellCommand = path;
            }
        }
        catch (Exception ex)
        {
            Log.ForContext<TerminalView>().Error(ex, "Failed to open file picker for bell command");
        }
    }
}
