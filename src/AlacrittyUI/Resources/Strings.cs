using System.Globalization;
using System.Resources;

namespace AlacrittyUI.Resources;

public static class Strings
{
    private static readonly ResourceManager Rm =
        new("AlacrittyUI.Resources.Strings", typeof(Strings).Assembly);

    private static string Get(string key) =>
        Rm.GetString(key, CultureInfo.CurrentUICulture) ?? key;

    // Tabs
    public static string TabColors => Get(nameof(TabColors));
    public static string TabFont => Get(nameof(TabFont));
    public static string TabWindow => Get(nameof(TabWindow));
    public static string TabCursor => Get(nameof(TabCursor));
    public static string TabTerminal => Get(nameof(TabTerminal));
    public static string TabThemes => Get(nameof(TabThemes));
    public static string TabInfo => Get(nameof(TabInfo));
    public static string SubTabPalette => Get(nameof(SubTabPalette));
    public static string SubTabSpecial => Get(nameof(SubTabSpecial));
    public static string SubTabExtended => Get(nameof(SubTabExtended));

    // Section headers
    public static string SectionPrimary => Get(nameof(SectionPrimary));
    public static string SectionNormal => Get(nameof(SectionNormal));
    public static string SectionBright => Get(nameof(SectionBright));
    public static string SectionCursor => Get(nameof(SectionCursor));
    public static string SectionSelection => Get(nameof(SectionSelection));
    public static string SectionSearch => Get(nameof(SectionSearch));
    public static string SectionFooter => Get(nameof(SectionFooter));
    public static string SectionFlags => Get(nameof(SectionFlags));

    // Buttons
    public static string ButtonSave => Get(nameof(ButtonSave));
    public static string ButtonOpen => Get(nameof(ButtonOpen));
    public static string ButtonApply => Get(nameof(ButtonApply));
    public static string ButtonImport => Get(nameof(ButtonImport));
    public static string ButtonExport => Get(nameof(ButtonExport));
    public static string ButtonDelete => Get(nameof(ButtonDelete));
    public static string ButtonSaveAsTheme => Get(nameof(ButtonSaveAsTheme));

    // Status
    public static string StatusReady => Get(nameof(StatusReady));
    public static string StatusSaved => Get(nameof(StatusSaved));
    public static string StatusLoaded => Get(nameof(StatusLoaded));
    public static string StatusNoConfig => Get(nameof(StatusNoConfig));
    public static string StatusThemeApplied => Get(nameof(StatusThemeApplied));

    // Toggles
    public static string ToggleBoldBright => Get(nameof(ToggleBoldBright));
    public static string ToggleTransparentBg => Get(nameof(ToggleTransparentBg));

    // Info
    public static string InfoDescription => Get(nameof(InfoDescription));
    public static string InfoDeveloper => Get(nameof(InfoDeveloper));
    public static string InfoAlacrittyVersion => Get(nameof(InfoAlacrittyVersion));
    public static string InfoLanguage => Get(nameof(InfoLanguage));
    public static string InfoRestartHint => Get(nameof(InfoRestartHint));

    // Theme manager
    public static string ThemeBuiltIn => Get(nameof(ThemeBuiltIn));
    public static string ThemeUser => Get(nameof(ThemeUser));
    public static string ThemeNamePrompt => Get(nameof(ThemeNamePrompt));

    // Font
    public static string FontSize => Get(nameof(FontSize));
    public static string FontNormal => Get(nameof(FontNormal));
    public static string FontBold => Get(nameof(FontBold));
    public static string FontItalic => Get(nameof(FontItalic));
    public static string FontBoldItalic => Get(nameof(FontBoldItalic));
    public static string FontTabMain => Get(nameof(FontTabMain));
    public static string FontTabSpacing => Get(nameof(FontTabSpacing));
    public static string FontFamilies => Get(nameof(FontFamilies));
    public static string FontOffset => Get(nameof(FontOffset));
    public static string FontGlyphOffset => Get(nameof(FontGlyphOffset));
    public static string FontBuiltinBoxDrawing => Get(nameof(FontBuiltinBoxDrawing));

    // Window
    public static string WindowTabLayout => Get(nameof(WindowTabLayout));
    public static string WindowTabAppearance => Get(nameof(WindowTabAppearance));
    public static string WindowDimensions => Get(nameof(WindowDimensions));
    public static string WindowDimensionsHint => Get(nameof(WindowDimensionsHint));
    public static string WindowColumns => Get(nameof(WindowColumns));
    public static string WindowLines => Get(nameof(WindowLines));
    public static string WindowDynamicPadding => Get(nameof(WindowDynamicPadding));
    public static string WindowOpacity => Get(nameof(WindowOpacity));
    public static string WindowBlur => Get(nameof(WindowBlur));
    public static string WindowDecorations => Get(nameof(WindowDecorations));
    public static string WindowStartupMode => Get(nameof(WindowStartupMode));
    public static string WindowTitle => Get(nameof(WindowTitle));
    public static string WindowDynamicTitle => Get(nameof(WindowDynamicTitle));
    public static string WindowResizeIncrements => Get(nameof(WindowResizeIncrements));
    public static string WindowLevel => Get(nameof(WindowLevel));
    public static string WindowThemeVariant => Get(nameof(WindowThemeVariant));
    public static string WindowOptionAsAlt => Get(nameof(WindowOptionAsAlt));
    public static string WindowPosition => Get(nameof(WindowPosition));
    public static string WindowSetPosition => Get(nameof(WindowSetPosition));
    public static string WindowClass => Get(nameof(WindowClass));

    // Cursor
    public static string CursorShape => Get(nameof(CursorShape));
    public static string CursorBlinking => Get(nameof(CursorBlinking));
    public static string CursorBlinkInterval => Get(nameof(CursorBlinkInterval));
    public static string CursorBlinkTimeout => Get(nameof(CursorBlinkTimeout));
    public static string CursorBlinkTimeoutHint => Get(nameof(CursorBlinkTimeoutHint));
    public static string CursorThickness => Get(nameof(CursorThickness));
    public static string CursorUnfocusedHollow => Get(nameof(CursorUnfocusedHollow));
    public static string CursorViModeStyle => Get(nameof(CursorViModeStyle));
    public static string CursorViModeEnabled => Get(nameof(CursorViModeEnabled));

    // Terminal
    public static string TermScrolling => Get(nameof(TermScrolling));
    public static string TermHistory => Get(nameof(TermHistory));
    public static string TermMultiplier => Get(nameof(TermMultiplier));
    public static string TermShellHint => Get(nameof(TermShellHint));
    public static string TermProgram => Get(nameof(TermProgram));
    public static string TermArgs => Get(nameof(TermArgs));
    public static string TermSelection => Get(nameof(TermSelection));
    public static string TermSaveToClipboard => Get(nameof(TermSaveToClipboard));
    public static string TermMouse => Get(nameof(TermMouse));
    public static string TermHideMouseWhenTyping => Get(nameof(TermHideMouseWhenTyping));
    public static string TermBell => Get(nameof(TermBell));
    public static string TermBellAnimation => Get(nameof(TermBellAnimation));
    public static string TermBellDuration => Get(nameof(TermBellDuration));
    public static string TermGeneral => Get(nameof(TermGeneral));
    public static string TermTabBellImport => Get(nameof(TermTabBellImport));
    public static string TermLiveReload => Get(nameof(TermLiveReload));
    public static string TermIpcSocket => Get(nameof(TermIpcSocket));
    public static string TermWorkingDir => Get(nameof(TermWorkingDir));
    public static string TermImport => Get(nameof(TermImport));
    public static string TermImportHint => Get(nameof(TermImportHint));

    // Tabs (Phase 3)
    public static string TabHints => Get(nameof(TabHints));
    public static string TabKeyboard => Get(nameof(TabKeyboard));
    public static string TabDebug => Get(nameof(TabDebug));

    // Hints
    public static string HintsAlphabet => Get(nameof(HintsAlphabet));

    // Buttons
    public static string ButtonAdd => Get(nameof(ButtonAdd));
    public static string ButtonRemove => Get(nameof(ButtonRemove));

    // Color sections (new)
    public static string SectionViModeCursor => Get(nameof(SectionViModeCursor));
    public static string SectionHintsColors => Get(nameof(SectionHintsColors));
    public static string SectionLineIndicator => Get(nameof(SectionLineIndicator));
    public static string SectionDim => Get(nameof(SectionDim));

    // Debug
    public static string DebugWarning => Get(nameof(DebugWarning));
    public static string DebugLogLevel => Get(nameof(DebugLogLevel));
    public static string DebugRenderer => Get(nameof(DebugRenderer));
    public static string DebugRenderTimer => Get(nameof(DebugRenderTimer));
    public static string DebugPersistentLogging => Get(nameof(DebugPersistentLogging));
    public static string DebugPrintEvents => Get(nameof(DebugPrintEvents));
    public static string DebugHighlightDamage => Get(nameof(DebugHighlightDamage));
    public static string DebugPreferEgl => Get(nameof(DebugPreferEgl));

    // Status errors
    public static string StatusLoadError => Get(nameof(StatusLoadError));
    public static string StatusSaveError => Get(nameof(StatusSaveError));
    public static string StatusThemeError => Get(nameof(StatusThemeError));

    // Shared labels
    public static string LabelFamily => Get(nameof(LabelFamily));
    public static string LabelStyle => Get(nameof(LabelStyle));
    public static string LabelPadding => Get(nameof(LabelPadding));
    public static string LabelWebsite => Get(nameof(LabelWebsite));
    public static string LabelRepository => Get(nameof(LabelRepository));
    public static string LabelShell => Get(nameof(LabelShell));
    public static string LabelCommand => Get(nameof(LabelCommand));
    public static string LabelRegex => Get(nameof(LabelRegex));
    public static string LabelAction => Get(nameof(LabelAction));
    public static string LabelKey => Get(nameof(LabelKey));
    public static string LabelMods => Get(nameof(LabelMods));
    public static string LabelMode => Get(nameof(LabelMode));
    public static string LabelChars => Get(nameof(LabelChars));
    public static string LabelHyperlinks => Get(nameof(LabelHyperlinks));
    public static string LabelPostProcessing => Get(nameof(LabelPostProcessing));
    public static string LabelPersist => Get(nameof(LabelPersist));
    public static string LabelMouseEnabled => Get(nameof(LabelMouseEnabled));
    public static string LabelMouseMods => Get(nameof(LabelMouseMods));
    public static string LabelBinding => Get(nameof(LabelBinding));
    public static string LabelOsc52 => Get(nameof(LabelOsc52));
    public static string LabelMacOsOnly => Get(nameof(LabelMacOsOnly));
    public static string LabelLinuxBsdOnly => Get(nameof(LabelLinuxBsdOnly));
    public static string LabelInstance => Get(nameof(LabelInstance));
    public static string LabelGeneral => Get(nameof(LabelGeneral));

    // Dialogs
    public static string DialogOpenConfig => Get(nameof(DialogOpenConfig));
    public static string DialogImportTheme => Get(nameof(DialogImportTheme));
    public static string DialogExportTheme => Get(nameof(DialogExportTheme));

    // Unsaved changes
    public static string UnsavedTitle => Get(nameof(UnsavedTitle));
    public static string UnsavedMessage => Get(nameof(UnsavedMessage));
    public static string ButtonDiscard => Get(nameof(ButtonDiscard));

    // Reset
    public static string ButtonReset => Get(nameof(ButtonReset));
    public static string ResetTitle => Get(nameof(ResetTitle));
    public static string ResetMessage => Get(nameof(ResetMessage));
    public static string ButtonCancel => Get(nameof(ButtonCancel));
    public static string ButtonContinue => Get(nameof(ButtonContinue));
    public static string StatusReset => Get(nameof(StatusReset));

    // Header
    public static string LabelConfig => Get(nameof(LabelConfig));

    // Color labels
    public static string ColorForeground => Get(nameof(ColorForeground));
    public static string ColorBackground => Get(nameof(ColorBackground));
    public static string ColorDimForeground => Get(nameof(ColorDimForeground));
    public static string ColorBrightForeground => Get(nameof(ColorBrightForeground));
    public static string ColorText => Get(nameof(ColorText));
    public static string ColorMatchFg => Get(nameof(ColorMatchFg));
    public static string ColorMatchBg => Get(nameof(ColorMatchBg));
    public static string ColorFocusedFg => Get(nameof(ColorFocusedFg));
    public static string ColorFocusedBg => Get(nameof(ColorFocusedBg));
    public static string ColorStartFg => Get(nameof(ColorStartFg));
    public static string ColorStartBg => Get(nameof(ColorStartBg));
    public static string ColorEndFg => Get(nameof(ColorEndFg));
    public static string ColorEndBg => Get(nameof(ColorEndBg));

    // ANSI color names
    public static string ColorBlack => Get(nameof(ColorBlack));
    public static string ColorRed => Get(nameof(ColorRed));
    public static string ColorGreen => Get(nameof(ColorGreen));
    public static string ColorYellow => Get(nameof(ColorYellow));
    public static string ColorBlue => Get(nameof(ColorBlue));
    public static string ColorMagenta => Get(nameof(ColorMagenta));
    public static string ColorCyan => Get(nameof(ColorCyan));
    public static string ColorWhite => Get(nameof(ColorWhite));
    public static string ColorCursor => Get(nameof(ColorCursor));

    // Watermarks
    public static string WatermarkFallbackNormal => Get(nameof(WatermarkFallbackNormal));
}
