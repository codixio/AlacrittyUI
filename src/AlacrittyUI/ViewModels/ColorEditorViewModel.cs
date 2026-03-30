using System.Collections.ObjectModel;
using AlacrittyUI.Models;
using AlacrittyUI.Resources;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AlacrittyUI.ViewModels;

public partial class ColorEditorViewModel : ObservableObject
{
    public ObservableCollection<ColorEntry> PrimaryColors { get; } = [];
    public ObservableCollection<ColorEntry> NormalColors { get; } = [];
    public ObservableCollection<ColorEntry> BrightColors { get; } = [];
    public ObservableCollection<ColorEntry> CursorColors { get; } = [];
    public ObservableCollection<ColorEntry> SelectionColors { get; } = [];
    public ObservableCollection<ColorEntry> SearchColors { get; } = [];
    public ObservableCollection<ColorEntry> FooterColors { get; } = [];
    public ObservableCollection<ColorEntry> ViModeCursorColors { get; } = [];
    public ObservableCollection<ColorEntry> HintsColors { get; } = [];
    public ObservableCollection<ColorEntry> LineIndicatorColors { get; } = [];
    public ObservableCollection<ColorEntry> DimColors { get; } = [];

    [ObservableProperty]
    private bool _drawBoldTextWithBrightColors;

    [ObservableProperty]
    private bool _transparentBackgroundColors;

    public void LoadFromPalette(ColorPalette p)
    {
        PrimaryColors.Clear();
        PrimaryColors.Add(new ColorEntry(Strings.ColorForeground, p.Foreground, "foreground"));
        PrimaryColors.Add(new ColorEntry(Strings.ColorBackground, p.Background, "background"));
        if (p.DimForeground != null)
            PrimaryColors.Add(new ColorEntry(Strings.ColorDimForeground, p.DimForeground, "dim_foreground"));
        if (p.BrightForeground != null)
            PrimaryColors.Add(new ColorEntry(Strings.ColorBrightForeground, p.BrightForeground, "bright_foreground"));

        NormalColors.Clear();
        NormalColors.Add(new ColorEntry(Strings.ColorBlack, p.NormalBlack, "normal.black"));
        NormalColors.Add(new ColorEntry(Strings.ColorRed, p.NormalRed, "normal.red"));
        NormalColors.Add(new ColorEntry(Strings.ColorGreen, p.NormalGreen, "normal.green"));
        NormalColors.Add(new ColorEntry(Strings.ColorYellow, p.NormalYellow, "normal.yellow"));
        NormalColors.Add(new ColorEntry(Strings.ColorBlue, p.NormalBlue, "normal.blue"));
        NormalColors.Add(new ColorEntry(Strings.ColorMagenta, p.NormalMagenta, "normal.magenta"));
        NormalColors.Add(new ColorEntry(Strings.ColorCyan, p.NormalCyan, "normal.cyan"));
        NormalColors.Add(new ColorEntry(Strings.ColorWhite, p.NormalWhite, "normal.white"));

        BrightColors.Clear();
        BrightColors.Add(new ColorEntry(Strings.ColorBlack, p.BrightBlack, "bright.black"));
        BrightColors.Add(new ColorEntry(Strings.ColorRed, p.BrightRed, "bright.red"));
        BrightColors.Add(new ColorEntry(Strings.ColorGreen, p.BrightGreen, "bright.green"));
        BrightColors.Add(new ColorEntry(Strings.ColorYellow, p.BrightYellow, "bright.yellow"));
        BrightColors.Add(new ColorEntry(Strings.ColorBlue, p.BrightBlue, "bright.blue"));
        BrightColors.Add(new ColorEntry(Strings.ColorMagenta, p.BrightMagenta, "bright.magenta"));
        BrightColors.Add(new ColorEntry(Strings.ColorCyan, p.BrightCyan, "bright.cyan"));
        BrightColors.Add(new ColorEntry(Strings.ColorWhite, p.BrightWhite, "bright.white"));

        CursorColors.Clear();
        CursorColors.Add(new ColorEntry(Strings.ColorText, p.CursorText ?? "#181818", "cursor.text"));
        CursorColors.Add(new ColorEntry(Strings.ColorCursor, p.CursorColor ?? "#d8d8d8", "cursor.cursor"));

        SelectionColors.Clear();
        SelectionColors.Add(new ColorEntry(Strings.ColorText, p.SelectionText ?? "#181818", "selection.text"));
        SelectionColors.Add(new ColorEntry(Strings.ColorBackground, p.SelectionBackground ?? "#d8d8d8", "selection.background"));

        SearchColors.Clear();
        SearchColors.Add(new ColorEntry(Strings.ColorMatchFg, p.SearchMatchForeground ?? "#181818", "search.matches.fg"));
        SearchColors.Add(new ColorEntry(Strings.ColorMatchBg, p.SearchMatchBackground ?? "#ac4242", "search.matches.bg"));
        SearchColors.Add(new ColorEntry(Strings.ColorFocusedFg, p.SearchFocusedForeground ?? "#181818", "search.focused.fg"));
        SearchColors.Add(new ColorEntry(Strings.ColorFocusedBg, p.SearchFocusedBackground ?? "#f4bf75", "search.focused.bg"));

        FooterColors.Clear();
        FooterColors.Add(new ColorEntry(Strings.ColorForeground, p.FooterBarForeground ?? "#181818", "footer.fg"));
        FooterColors.Add(new ColorEntry(Strings.ColorBackground, p.FooterBarBackground ?? "#d8d8d8", "footer.bg"));

        ViModeCursorColors.Clear();
        ViModeCursorColors.Add(new ColorEntry(Strings.ColorText, p.ViModeCursorText ?? "#181818", "vi_cursor.text"));
        ViModeCursorColors.Add(new ColorEntry(Strings.ColorCursor, p.ViModeCursorColor ?? "#d8d8d8", "vi_cursor.cursor"));

        HintsColors.Clear();
        HintsColors.Add(new ColorEntry(Strings.ColorStartFg, p.HintsStartForeground ?? "#181818", "hints.start.fg"));
        HintsColors.Add(new ColorEntry(Strings.ColorStartBg, p.HintsStartBackground ?? "#f4bf75", "hints.start.bg"));
        HintsColors.Add(new ColorEntry(Strings.ColorEndFg, p.HintsEndForeground ?? "#181818", "hints.end.fg"));
        HintsColors.Add(new ColorEntry(Strings.ColorEndBg, p.HintsEndBackground ?? "#ac4242", "hints.end.bg"));

        LineIndicatorColors.Clear();
        LineIndicatorColors.Add(new ColorEntry(Strings.ColorForeground, p.LineIndicatorForeground ?? "#ffffff", "line.fg"));
        LineIndicatorColors.Add(new ColorEntry(Strings.ColorBackground, p.LineIndicatorBackground ?? "#ffffff", "line.bg"));

        DimColors.Clear();
        DimColors.Add(new ColorEntry(Strings.ColorBlack, p.DimBlack ?? "#0f0f0f", "dim.black"));
        DimColors.Add(new ColorEntry(Strings.ColorRed, p.DimRed ?? "#712b2b", "dim.red"));
        DimColors.Add(new ColorEntry(Strings.ColorGreen, p.DimGreen ?? "#5f6f3a", "dim.green"));
        DimColors.Add(new ColorEntry(Strings.ColorYellow, p.DimYellow ?? "#a17e4d", "dim.yellow"));
        DimColors.Add(new ColorEntry(Strings.ColorBlue, p.DimBlue ?? "#456877", "dim.blue"));
        DimColors.Add(new ColorEntry(Strings.ColorMagenta, p.DimMagenta ?? "#704d68", "dim.magenta"));
        DimColors.Add(new ColorEntry(Strings.ColorCyan, p.DimCyan ?? "#4d7770", "dim.cyan"));
        DimColors.Add(new ColorEntry(Strings.ColorWhite, p.DimWhite ?? "#8e8e8e", "dim.white"));

        DrawBoldTextWithBrightColors = p.DrawBoldTextWithBrightColors;
        TransparentBackgroundColors = p.TransparentBackgroundColors;
    }

    public void ApplyToPalette(ColorPalette p)
    {
        if (PrimaryColors.Count > 0) p.Foreground = PrimaryColors[0].HexValue;
        if (PrimaryColors.Count > 1) p.Background = PrimaryColors[1].HexValue;
        if (PrimaryColors.Count > 2) p.DimForeground = PrimaryColors[2].HexValue;
        if (PrimaryColors.Count > 3) p.BrightForeground = PrimaryColors[3].HexValue;

        if (NormalColors.Count == 8)
        {
            p.NormalBlack = NormalColors[0].HexValue;
            p.NormalRed = NormalColors[1].HexValue;
            p.NormalGreen = NormalColors[2].HexValue;
            p.NormalYellow = NormalColors[3].HexValue;
            p.NormalBlue = NormalColors[4].HexValue;
            p.NormalMagenta = NormalColors[5].HexValue;
            p.NormalCyan = NormalColors[6].HexValue;
            p.NormalWhite = NormalColors[7].HexValue;
        }

        if (BrightColors.Count == 8)
        {
            p.BrightBlack = BrightColors[0].HexValue;
            p.BrightRed = BrightColors[1].HexValue;
            p.BrightGreen = BrightColors[2].HexValue;
            p.BrightYellow = BrightColors[3].HexValue;
            p.BrightBlue = BrightColors[4].HexValue;
            p.BrightMagenta = BrightColors[5].HexValue;
            p.BrightCyan = BrightColors[6].HexValue;
            p.BrightWhite = BrightColors[7].HexValue;
        }

        if (CursorColors.Count == 2)
        {
            p.CursorText = CursorColors[0].HexValue;
            p.CursorColor = CursorColors[1].HexValue;
        }

        if (SelectionColors.Count == 2)
        {
            p.SelectionText = SelectionColors[0].HexValue;
            p.SelectionBackground = SelectionColors[1].HexValue;
        }

        if (SearchColors.Count == 4)
        {
            p.SearchMatchForeground = SearchColors[0].HexValue;
            p.SearchMatchBackground = SearchColors[1].HexValue;
            p.SearchFocusedForeground = SearchColors[2].HexValue;
            p.SearchFocusedBackground = SearchColors[3].HexValue;
        }

        if (FooterColors.Count == 2)
        {
            p.FooterBarForeground = FooterColors[0].HexValue;
            p.FooterBarBackground = FooterColors[1].HexValue;
        }

        if (ViModeCursorColors.Count == 2)
        {
            p.ViModeCursorText = ViModeCursorColors[0].HexValue;
            p.ViModeCursorColor = ViModeCursorColors[1].HexValue;
        }

        if (HintsColors.Count == 4)
        {
            p.HintsStartForeground = HintsColors[0].HexValue;
            p.HintsStartBackground = HintsColors[1].HexValue;
            p.HintsEndForeground = HintsColors[2].HexValue;
            p.HintsEndBackground = HintsColors[3].HexValue;
        }

        if (LineIndicatorColors.Count == 2)
        {
            p.LineIndicatorForeground = LineIndicatorColors[0].HexValue;
            p.LineIndicatorBackground = LineIndicatorColors[1].HexValue;
        }

        if (DimColors.Count == 8)
        {
            p.DimBlack = DimColors[0].HexValue;
            p.DimRed = DimColors[1].HexValue;
            p.DimGreen = DimColors[2].HexValue;
            p.DimYellow = DimColors[3].HexValue;
            p.DimBlue = DimColors[4].HexValue;
            p.DimMagenta = DimColors[5].HexValue;
            p.DimCyan = DimColors[6].HexValue;
            p.DimWhite = DimColors[7].HexValue;
        }

        p.DrawBoldTextWithBrightColors = DrawBoldTextWithBrightColors;
        p.TransparentBackgroundColors = TransparentBackgroundColors;
    }
}
