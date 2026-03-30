using System.Text;
using AlacrittyUI.Models;
using Serilog;
using Tomlyn;
using Tomlyn.Model;

namespace AlacrittyUI.Services;

public class ConfigReaderService
{
    private static readonly ILogger Logger = Log.ForContext<ConfigReaderService>();

    public AlacrittyConfig ReadFromFile(string path)
    {
        try
        {
            Logger.Information("Reading config from {Path}", path);
            var tomlContent = File.ReadAllText(path, Encoding.UTF8);
            return ReadFromString(tomlContent);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to read config from {Path}", path);
            throw;
        }
    }

    public AlacrittyConfig ReadFromString(string tomlContent)
    {
        TomlTable doc;
        try
        {
            doc = TomlSerializer.Deserialize<TomlTable>(tomlContent) ?? new TomlTable();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to parse TOML content");
            throw;
        }
        var config = new AlacrittyConfig { RawDocument = doc };

        if (doc.TryGetValue("colors", out var colorsObj) && colorsObj is TomlTable colors)
            ReadColors(colors, config.Colors);

        ReadFont(doc, config.Font);
        ReadWindow(doc, config.Window);
        ReadCursor(doc, config.Cursor);
        ReadTerminal(doc, config.Terminal);
        ReadHints(doc, config.Hints);
        ReadKeyboard(doc, config.Keyboard);
        ReadDebug(doc, config.Debug);

        return config;
    }

    private static void ReadColors(TomlTable colors, ColorPalette palette)
    {
        if (colors.TryGetValue("primary", out var primaryObj) && primaryObj is TomlTable primary)
        {
            palette.Foreground = GetString(primary, "foreground", palette.Foreground);
            palette.Background = GetString(primary, "background", palette.Background);
            palette.DimForeground = GetStringOrNull(primary, "dim_foreground") ?? palette.DimForeground;
            palette.BrightForeground = GetStringOrNull(primary, "bright_foreground");
        }

        if (colors.TryGetValue("normal", out var normalObj) && normalObj is TomlTable normal)
        {
            palette.NormalBlack = GetString(normal, "black", palette.NormalBlack);
            palette.NormalRed = GetString(normal, "red", palette.NormalRed);
            palette.NormalGreen = GetString(normal, "green", palette.NormalGreen);
            palette.NormalYellow = GetString(normal, "yellow", palette.NormalYellow);
            palette.NormalBlue = GetString(normal, "blue", palette.NormalBlue);
            palette.NormalMagenta = GetString(normal, "magenta", palette.NormalMagenta);
            palette.NormalCyan = GetString(normal, "cyan", palette.NormalCyan);
            palette.NormalWhite = GetString(normal, "white", palette.NormalWhite);
        }

        if (colors.TryGetValue("bright", out var brightObj) && brightObj is TomlTable bright)
        {
            palette.BrightBlack = GetString(bright, "black", palette.BrightBlack);
            palette.BrightRed = GetString(bright, "red", palette.BrightRed);
            palette.BrightGreen = GetString(bright, "green", palette.BrightGreen);
            palette.BrightYellow = GetString(bright, "yellow", palette.BrightYellow);
            palette.BrightBlue = GetString(bright, "blue", palette.BrightBlue);
            palette.BrightMagenta = GetString(bright, "magenta", palette.BrightMagenta);
            palette.BrightCyan = GetString(bright, "cyan", palette.BrightCyan);
            palette.BrightWhite = GetString(bright, "white", palette.BrightWhite);
        }

        if (colors.TryGetValue("cursor", out var cursorObj) && cursorObj is TomlTable cursor)
        {
            palette.CursorText = GetStringOrNull(cursor, "text");
            palette.CursorColor = GetStringOrNull(cursor, "cursor");
        }

        if (colors.TryGetValue("selection", out var selObj) && selObj is TomlTable selection)
        {
            palette.SelectionText = GetStringOrNull(selection, "text");
            palette.SelectionBackground = GetStringOrNull(selection, "background");
        }

        if (colors.TryGetValue("search", out var searchObj) && searchObj is TomlTable search)
        {
            if (search.TryGetValue("matches", out var matchesObj) && matchesObj is TomlTable matches)
            {
                palette.SearchMatchForeground = GetStringOrNull(matches, "foreground");
                palette.SearchMatchBackground = GetStringOrNull(matches, "background");
            }

            if (search.TryGetValue("focused_match", out var focusedObj) && focusedObj is TomlTable focused)
            {
                palette.SearchFocusedForeground = GetStringOrNull(focused, "foreground");
                palette.SearchFocusedBackground = GetStringOrNull(focused, "background");
            }
        }

        if (colors.TryGetValue("footer_bar", out var footerObj) && footerObj is TomlTable footer)
        {
            palette.FooterBarForeground = GetStringOrNull(footer, "foreground");
            palette.FooterBarBackground = GetStringOrNull(footer, "background");
        }

        if (colors.TryGetValue("vi_mode_cursor", out var viCursorObj) && viCursorObj is TomlTable viCursor)
        {
            palette.ViModeCursorText = GetStringOrNull(viCursor, "text");
            palette.ViModeCursorColor = GetStringOrNull(viCursor, "cursor");
        }

        if (colors.TryGetValue("hints", out var hintsObj) && hintsObj is TomlTable hintsColors)
        {
            if (hintsColors.TryGetValue("start", out var startObj) && startObj is TomlTable start)
            {
                palette.HintsStartForeground = GetStringOrNull(start, "foreground");
                palette.HintsStartBackground = GetStringOrNull(start, "background");
            }

            if (hintsColors.TryGetValue("end", out var endObj) && endObj is TomlTable end)
            {
                palette.HintsEndForeground = GetStringOrNull(end, "foreground");
                palette.HintsEndBackground = GetStringOrNull(end, "background");
            }
        }

        if (colors.TryGetValue("line_indicator", out var lineIndObj) && lineIndObj is TomlTable lineInd)
        {
            palette.LineIndicatorForeground = GetStringOrNull(lineInd, "foreground");
            palette.LineIndicatorBackground = GetStringOrNull(lineInd, "background");
        }

        if (colors.TryGetValue("dim", out var dimObj) && dimObj is TomlTable dim)
        {
            palette.DimBlack = GetStringOrNull(dim, "black");
            palette.DimRed = GetStringOrNull(dim, "red");
            palette.DimGreen = GetStringOrNull(dim, "green");
            palette.DimYellow = GetStringOrNull(dim, "yellow");
            palette.DimBlue = GetStringOrNull(dim, "blue");
            palette.DimMagenta = GetStringOrNull(dim, "magenta");
            palette.DimCyan = GetStringOrNull(dim, "cyan");
            palette.DimWhite = GetStringOrNull(dim, "white");
        }

        if (colors.TryGetValue("draw_bold_text_with_bright_colors", out var boldBright))
            palette.DrawBoldTextWithBrightColors = boldBright is true;

        if (colors.TryGetValue("transparent_background_colors", out var transBg))
            palette.TransparentBackgroundColors = transBg is true;
    }

    private static string GetString(TomlTable table, string key, string defaultValue)
        => table.TryGetValue(key, out var val) && val is string s ? s : defaultValue;

    private static string? GetStringOrNull(TomlTable table, string key)
        => table.TryGetValue(key, out var val) && val is string s ? s : null;

    private static int GetInt(TomlTable table, string key, int defaultValue)
        => table.TryGetValue(key, out var val) && val is long l ? (int)l : defaultValue;

    private static double GetDouble(TomlTable table, string key, double defaultValue)
        => table.TryGetValue(key, out var val) && val is double d ? d : defaultValue;

    private static bool GetBool(TomlTable table, string key, bool defaultValue)
        => table.TryGetValue(key, out var val) && val is bool b ? b : defaultValue;

    private static void ReadFont(TomlTable doc, FontConfig font)
    {
        if (!doc.TryGetValue("font", out var fontObj) || fontObj is not TomlTable fontTable)
            return;

        font.Size = GetDouble(fontTable, "size", font.Size);
        font.BuiltinBoxDrawing = GetBool(fontTable, "builtin_box_drawing", font.BuiltinBoxDrawing);

        if (fontTable.TryGetValue("normal", out var normalObj) && normalObj is TomlTable normal)
        {
            font.NormalFamily = GetString(normal, "family", font.NormalFamily);
            font.NormalStyle = GetString(normal, "style", font.NormalStyle);
        }

        if (fontTable.TryGetValue("bold", out var boldObj) && boldObj is TomlTable bold)
        {
            font.BoldFamily = GetStringOrNull(bold, "family") ?? font.BoldFamily;
            font.BoldStyle = GetString(bold, "style", font.BoldStyle);
        }

        if (fontTable.TryGetValue("italic", out var italicObj) && italicObj is TomlTable italic)
        {
            font.ItalicFamily = GetStringOrNull(italic, "family") ?? font.ItalicFamily;
            font.ItalicStyle = GetString(italic, "style", font.ItalicStyle);
        }

        if (fontTable.TryGetValue("bold_italic", out var boldItalicObj) && boldItalicObj is TomlTable boldItalic)
        {
            font.BoldItalicFamily = GetStringOrNull(boldItalic, "family") ?? font.BoldItalicFamily;
            font.BoldItalicStyle = GetString(boldItalic, "style", font.BoldItalicStyle);
        }

        if (fontTable.TryGetValue("offset", out var offsetObj) && offsetObj is TomlTable offset)
        {
            font.OffsetX = GetInt(offset, "x", font.OffsetX);
            font.OffsetY = GetInt(offset, "y", font.OffsetY);
        }

        if (fontTable.TryGetValue("glyph_offset", out var glyphObj) && glyphObj is TomlTable glyph)
        {
            font.GlyphOffsetX = GetInt(glyph, "x", font.GlyphOffsetX);
            font.GlyphOffsetY = GetInt(glyph, "y", font.GlyphOffsetY);
        }
    }

    private static void ReadWindow(TomlTable doc, WindowConfig window)
    {
        if (!doc.TryGetValue("window", out var windowObj) || windowObj is not TomlTable windowTable)
            return;

        window.Opacity = GetDouble(windowTable, "opacity", window.Opacity);
        window.Blur = GetBool(windowTable, "blur", window.Blur);
        window.DynamicPadding = GetBool(windowTable, "dynamic_padding", window.DynamicPadding);
        window.Decorations = GetString(windowTable, "decorations", window.Decorations);
        window.StartupMode = GetString(windowTable, "startup_mode", window.StartupMode);
        window.Title = GetString(windowTable, "title", window.Title);
        window.DynamicTitle = GetBool(windowTable, "dynamic_title", window.DynamicTitle);
        window.ResizeIncrements = GetBool(windowTable, "resize_increments", window.ResizeIncrements);

        if (windowTable.TryGetValue("dimensions", out var dimObj) && dimObj is TomlTable dimensions)
        {
            window.Columns = GetInt(dimensions, "columns", window.Columns);
            window.Lines = GetInt(dimensions, "lines", window.Lines);
        }

        if (windowTable.TryGetValue("padding", out var padObj) && padObj is TomlTable padding)
        {
            window.PaddingX = GetInt(padding, "x", window.PaddingX);
            window.PaddingY = GetInt(padding, "y", window.PaddingY);
        }

        if (windowTable.TryGetValue("position", out var posObj) && posObj is TomlTable pos)
        {
            window.PositionX = GetInt(pos, "x", 0);
            window.PositionY = GetInt(pos, "y", 0);
            window.HasPosition = true;
        }

        if (windowTable.TryGetValue("class", out var classObj) && classObj is TomlTable cls)
        {
            window.ClassInstance = GetString(cls, "instance", window.ClassInstance);
            window.ClassGeneral = GetString(cls, "general", window.ClassGeneral);
        }

        window.DecorationsThemeVariant = GetString(windowTable, "decorations_theme_variant", window.DecorationsThemeVariant);
        window.OptionAsAlt = GetString(windowTable, "option_as_alt", window.OptionAsAlt);
        window.Level = GetString(windowTable, "level", window.Level);
    }

    private static void ReadCursor(TomlTable doc, CursorConfig cursor)
    {
        if (!doc.TryGetValue("cursor", out var cursorObj) || cursorObj is not TomlTable cursorTable)
            return;

        if (cursorTable.TryGetValue("vi_mode_style", out var viObj))
        {
            if (viObj is TomlTable viTable)
            {
                cursor.ViModeEnabled = true;
                cursor.ViModeShape = GetString(viTable, "shape", cursor.ViModeShape);
                cursor.ViModeBlinking = GetString(viTable, "blinking", cursor.ViModeBlinking);
            }
        }

        cursor.BlinkInterval = GetInt(cursorTable, "blink_interval", cursor.BlinkInterval);
        cursor.BlinkTimeout = GetInt(cursorTable, "blink_timeout", cursor.BlinkTimeout);
        cursor.UnfocusedHollow = GetBool(cursorTable, "unfocused_hollow", cursor.UnfocusedHollow);
        cursor.Thickness = GetDouble(cursorTable, "thickness", cursor.Thickness);

        if (cursorTable.TryGetValue("style", out var styleObj) && styleObj is TomlTable style)
        {
            cursor.Shape = GetString(style, "shape", cursor.Shape);
            cursor.Blinking = GetString(style, "blinking", cursor.Blinking);
        }
    }

    private static void ReadTerminal(TomlTable doc, TerminalConfig terminal)
    {
        if (doc.TryGetValue("scrolling", out var scrollObj) && scrollObj is TomlTable scrolling)
        {
            terminal.ScrollingHistory = GetInt(scrolling, "history", terminal.ScrollingHistory);
            terminal.ScrollingMultiplier = GetInt(scrolling, "multiplier", terminal.ScrollingMultiplier);
        }

        if (doc.TryGetValue("terminal", out var termObj) && termObj is TomlTable termTable)
        {
            terminal.Osc52 = GetString(termTable, "osc52", terminal.Osc52);

            if (termTable.TryGetValue("shell", out var shellObj) && shellObj is TomlTable shell)
            {
                terminal.ShellProgram = GetString(shell, "program", terminal.ShellProgram);
                if (shell.TryGetValue("args", out var argsObj) && argsObj is TomlArray args)
                    terminal.ShellArgs = string.Join(" ", args.Select(a => a?.ToString() ?? string.Empty));
            }
        }

        if (doc.TryGetValue("selection", out var selObj) && selObj is TomlTable selection)
        {
            terminal.SaveToClipboard = GetBool(selection, "save_to_clipboard", terminal.SaveToClipboard);
            terminal.SemanticEscapeChars = GetString(selection, "semantic_escape_chars", terminal.SemanticEscapeChars);
        }

        if (doc.TryGetValue("mouse", out var mouseObj) && mouseObj is TomlTable mouse)
            terminal.HideMouseWhenTyping = GetBool(mouse, "hide_when_typing", terminal.HideMouseWhenTyping);

        if (doc.TryGetValue("general", out var generalObj) && generalObj is TomlTable general)
        {
            terminal.LiveConfigReload = GetBool(general, "live_config_reload", terminal.LiveConfigReload);
            terminal.IpcSocket = GetBool(general, "ipc_socket", terminal.IpcSocket);
            terminal.WorkingDirectory = GetString(general, "working_directory", terminal.WorkingDirectory);

            if (general.TryGetValue("import", out var importObj) && importObj is TomlArray importArr)
                terminal.Import = importArr.OfType<string>().ToList();
        }

        if (doc.TryGetValue("bell", out var bellObj) && bellObj is TomlTable bell)
        {
            terminal.BellAnimation = GetString(bell, "animation", terminal.BellAnimation);
            terminal.BellDuration = GetInt(bell, "duration", terminal.BellDuration);
            terminal.BellColor = GetString(bell, "color", terminal.BellColor);

            if (bell.TryGetValue("command", out var cmdObj))
            {
                if (cmdObj is string cmdStr)
                    terminal.BellCommand = cmdStr;
                else if (cmdObj is TomlTable cmdTable)
                    terminal.BellCommand = GetString(cmdTable, "program", "");
            }
        }
    }

    private static void ReadHints(TomlTable doc, HintsConfig hints)
    {
        if (!doc.TryGetValue("hints", out var hintsObj) || hintsObj is not TomlTable hintsTable) return;

        hints.Alphabet = GetString(hintsTable, "alphabet", hints.Alphabet);

        if (hintsTable.TryGetValue("enabled", out var enabledObj) && enabledObj is TomlTableArray enabledArr)
        {
            foreach (var ruleTable in enabledArr)
            {
                var rule = new HintRule
                {
                    Regex = GetString(ruleTable, "regex", ""),
                    Hyperlinks = GetBool(ruleTable, "hyperlinks", false),
                    PostProcessing = GetBool(ruleTable, "post_processing", false),
                    Persist = GetBool(ruleTable, "persist", false),
                    Action = GetString(ruleTable, "action", ""),
                };

                if (ruleTable.TryGetValue("command", out var cmdVal))
                {
                    if (cmdVal is string cmdStr)
                        rule.Command = cmdStr;
                    else if (cmdVal is TomlTable cmdTable)
                        rule.Command = GetString(cmdTable, "program", "");
                }

                if (ruleTable.TryGetValue("binding", out var bindObj) && bindObj is TomlTable bindTable)
                {
                    rule.BindingKey = GetString(bindTable, "key", "");
                    rule.BindingMods = GetString(bindTable, "mods", "");
                }

                if (ruleTable.TryGetValue("mouse", out var mouseObj) && mouseObj is TomlTable mouseTable)
                {
                    rule.MouseEnabled = GetBool(mouseTable, "enabled", true);
                    rule.MouseMods = GetString(mouseTable, "mods", "");
                }

                hints.Enabled.Add(rule);
            }
        }
    }

    private static void ReadKeyboard(TomlTable doc, KeyboardConfig keyboard)
    {
        if (!doc.TryGetValue("keyboard", out var kbObj) || kbObj is not TomlTable kbTable) return;

        if (kbTable.TryGetValue("bindings", out var bindingsObj) && bindingsObj is TomlTableArray bindingsArr)
        {
            foreach (var b in bindingsArr)
            {
                var binding = new KeyBinding
                {
                    Key = GetString(b, "key", ""),
                    Mods = GetString(b, "mods", ""),
                    Mode = GetString(b, "mode", ""),
                    Action = GetString(b, "action", ""),
                    Chars = GetString(b, "chars", ""),
                };

                if (b.TryGetValue("command", out var cmdVal))
                {
                    if (cmdVal is string cmdStr)
                        binding.Command = cmdStr;
                    else if (cmdVal is TomlTable cmdTable)
                        binding.Command = GetString(cmdTable, "program", "");
                }

                keyboard.Bindings.Add(binding);
            }
        }
    }

    private static void ReadDebug(TomlTable doc, DebugConfig debug)
    {
        if (!doc.TryGetValue("debug", out var debugObj) || debugObj is not TomlTable debugTable) return;

        debug.RenderTimer = GetBool(debugTable, "render_timer", debug.RenderTimer);
        debug.PersistentLogging = GetBool(debugTable, "persistent_logging", debug.PersistentLogging);
        debug.LogLevel = GetString(debugTable, "log_level", debug.LogLevel);
        debug.Renderer = GetString(debugTable, "renderer", debug.Renderer);
        debug.PrintEvents = GetBool(debugTable, "print_events", debug.PrintEvents);
        debug.HighlightDamage = GetBool(debugTable, "highlight_damage", debug.HighlightDamage);
        debug.PreferEgl = GetBool(debugTable, "prefer_egl", debug.PreferEgl);
    }
}
