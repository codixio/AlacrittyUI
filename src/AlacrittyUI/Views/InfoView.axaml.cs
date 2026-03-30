using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Input;
using AlacrittyUI.ViewModels;
using Serilog;

namespace AlacrittyUI.Views;

public partial class InfoView : UserControl
{
    public InfoView()
    {
        InitializeComponent();
        AddHandler(PointerPressedEvent, OnPointerPressed, handledEventsToo: true);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is not TextBlock { Cursor: not null } textBlock) return;
        if (DataContext is not InfoViewModel vm) return;

        // match clicked text to a URL
        var text = GetTextBlockText(textBlock);
        var url = text switch
        {
            _ when text == vm.Website => vm.Website,
            _ when text == vm.RepoUrl => vm.RepoUrl,
            _ when text == vm.AlacrittyRepoUrl => vm.AlacrittyRepoUrl,
            _ => null
        };

        if (url != null)
            OpenUrl(url);
    }

    private static string GetTextBlockText(TextBlock tb)
    {
        // extract text from Inlines or Text property
        if (tb.Inlines != null)
        {
            return string.Concat(tb.Inlines.OfType<Avalonia.Controls.Documents.Run>()
                .Select(r => r.Text ?? ""));
        }
        return tb.Text ?? "";
    }

    private static void OpenUrl(string url)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Process.Start("open", url);
            else
                Process.Start("xdg-open", url);
        }
        catch (Exception ex)
        {
            Log.ForContext<InfoView>().Warning(ex, "Failed to open URL {Url}", url);
        }
    }
}
