using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.Models;

public partial class ThemeInfo : ObservableObject
{
    public required string Name { get; init; }
    public required string FilePath { get; init; }
    public bool IsBuiltIn { get; init; }
    public string? PreviewBackground { get; init; }
    public string? PreviewForeground { get; init; }

    public ISolidColorBrush BackgroundBrush =>
        Color.TryParse(PreviewBackground, out var c) ? new SolidColorBrush(c) : new SolidColorBrush(Color.Parse("#181818"));

    public ISolidColorBrush ForegroundBrush =>
        Color.TryParse(PreviewForeground, out var c) ? new SolidColorBrush(c) : new SolidColorBrush(Color.Parse("#d8d8d8"));

    [ObservableProperty]
    private bool _isActive;
}
