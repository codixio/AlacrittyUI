using AlacrittyUI.Helpers;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.Models;

public partial class ColorEntry : ObservableObject
{
    [ObservableProperty]
    private string _label = string.Empty;

    [ObservableProperty]
    private string _hexValue = "#000000";

    [ObservableProperty]
    private ISolidColorBrush _preview = new SolidColorBrush(Colors.Black);

    [ObservableProperty]
    private bool _hasError;

    public string Key { get; init; } = string.Empty;

    public ColorEntry()
    {
    }

    public ColorEntry(string label, string hexValue, string key)
    {
        _label = label;
        _hexValue = hexValue;
        Key = key;
        UpdatePreview();
    }

    partial void OnHexValueChanged(string value)
    {
        HasError = !ValidationHelper.IsValidHexColor(value);
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (Color.TryParse(HexValue, out var color))
            Preview = new SolidColorBrush(color);
    }
}
