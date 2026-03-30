using AlacrittyUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.ViewModels;

public partial class FontViewModel : ObservableObject
{
    [ObservableProperty] private double _size = 11.25;
    [ObservableProperty] private string _normalFamily = FontConfig.GetDefaultFontFamily();
    [ObservableProperty] private string _normalStyle = "Regular";
    [ObservableProperty] private string _boldFamily = string.Empty;
    [ObservableProperty] private string _boldStyle = "Bold";
    [ObservableProperty] private string _italicFamily = string.Empty;
    [ObservableProperty] private string _italicStyle = "Italic";
    [ObservableProperty] private string _boldItalicFamily = string.Empty;
    [ObservableProperty] private string _boldItalicStyle = "Bold Italic";
    [ObservableProperty] private int _offsetX;
    [ObservableProperty] private int _offsetY;
    [ObservableProperty] private int _glyphOffsetX;
    [ObservableProperty] private int _glyphOffsetY;
    [ObservableProperty] private bool _builtinBoxDrawing = true;

    public void LoadFrom(FontConfig f)
    {
        Size = f.Size;
        NormalFamily = f.NormalFamily;
        NormalStyle = f.NormalStyle;
        BoldFamily = f.BoldFamily ?? string.Empty;
        BoldStyle = f.BoldStyle;
        ItalicFamily = f.ItalicFamily ?? string.Empty;
        ItalicStyle = f.ItalicStyle;
        BoldItalicFamily = f.BoldItalicFamily ?? string.Empty;
        BoldItalicStyle = f.BoldItalicStyle;
        OffsetX = f.OffsetX;
        OffsetY = f.OffsetY;
        GlyphOffsetX = f.GlyphOffsetX;
        GlyphOffsetY = f.GlyphOffsetY;
        BuiltinBoxDrawing = f.BuiltinBoxDrawing;
    }

    public void ApplyTo(FontConfig f)
    {
        f.Size = Size;
        f.NormalFamily = NormalFamily;
        f.NormalStyle = NormalStyle;
        f.BoldFamily = string.IsNullOrWhiteSpace(BoldFamily) ? null : BoldFamily;
        f.BoldStyle = BoldStyle;
        f.ItalicFamily = string.IsNullOrWhiteSpace(ItalicFamily) ? null : ItalicFamily;
        f.ItalicStyle = ItalicStyle;
        f.BoldItalicFamily = string.IsNullOrWhiteSpace(BoldItalicFamily) ? null : BoldItalicFamily;
        f.BoldItalicStyle = BoldItalicStyle;
        f.OffsetX = OffsetX;
        f.OffsetY = OffsetY;
        f.GlyphOffsetX = GlyphOffsetX;
        f.GlyphOffsetY = GlyphOffsetY;
        f.BuiltinBoxDrawing = BuiltinBoxDrawing;
    }
}
