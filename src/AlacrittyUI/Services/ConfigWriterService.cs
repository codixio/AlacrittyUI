using System.Text;
using AlacrittyUI.Models;
using Serilog;
using Tomlyn;
using Tomlyn.Model;

namespace AlacrittyUI.Services;

public class ConfigWriterService
{
    private static readonly ILogger Logger = Log.ForContext<ConfigWriterService>();

    public void WriteConfig(string path, AlacrittyConfig config)
    {
        try
        {
            Logger.Information("Writing config to {Path}", path);

            CreateBackup(path);

            var doc = config.RawDocument ?? new TomlTable();

            if (!doc.ContainsKey("colors"))
                doc["colors"] = new TomlTable();

            var colors = (TomlTable)doc["colors"];
            WriteColors(colors, config.Colors);

            WriteFont(doc, config.Font);
            WriteWindow(doc, config.Window);
            WriteCursor(doc, config.Cursor);
            WriteTerminal(doc, config.Terminal);
            WriteHints(doc, config.Hints);
            WriteKeyboard(doc, config.Keyboard);
            WriteDebug(doc, config.Debug);

            var toml = TomlSerializer.Serialize(doc);
            // UTF-8 without BOM — TOML spec requires no BOM
            File.WriteAllText(path, toml, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            Logger.Information("Config saved to {Path}", path);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to write config to {Path}", path);
            throw;
        }
    }

    private static void WriteColors(TomlTable colors, ColorPalette palette)
    {
        var primary = GetOrCreateTable(colors, "primary");
        primary["foreground"] = palette.Foreground;
        primary["background"] = palette.Background;
        if (palette.DimForeground != null)
            primary["dim_foreground"] = palette.DimForeground;
        if (palette.BrightForeground != null)
            primary["bright_foreground"] = palette.BrightForeground;

        var normal = GetOrCreateTable(colors, "normal");
        normal["black"] = palette.NormalBlack;
        normal["red"] = palette.NormalRed;
        normal["green"] = palette.NormalGreen;
        normal["yellow"] = palette.NormalYellow;
        normal["blue"] = palette.NormalBlue;
        normal["magenta"] = palette.NormalMagenta;
        normal["cyan"] = palette.NormalCyan;
        normal["white"] = palette.NormalWhite;

        var bright = GetOrCreateTable(colors, "bright");
        bright["black"] = palette.BrightBlack;
        bright["red"] = palette.BrightRed;
        bright["green"] = palette.BrightGreen;
        bright["yellow"] = palette.BrightYellow;
        bright["blue"] = palette.BrightBlue;
        bright["magenta"] = palette.BrightMagenta;
        bright["cyan"] = palette.BrightCyan;
        bright["white"] = palette.BrightWhite;

        if (palette.CursorText != null || palette.CursorColor != null)
        {
            var cursor = GetOrCreateTable(colors, "cursor");
            if (palette.CursorText != null) cursor["text"] = palette.CursorText;
            if (palette.CursorColor != null) cursor["cursor"] = palette.CursorColor;
        }

        if (palette.SelectionText != null || palette.SelectionBackground != null)
        {
            var selection = GetOrCreateTable(colors, "selection");
            if (palette.SelectionText != null) selection["text"] = palette.SelectionText;
            if (palette.SelectionBackground != null) selection["background"] = palette.SelectionBackground;
        }

        if (palette.SearchMatchForeground != null || palette.SearchMatchBackground != null ||
            palette.SearchFocusedForeground != null || palette.SearchFocusedBackground != null)
        {
            var search = GetOrCreateTable(colors, "search");

            if (palette.SearchMatchForeground != null || palette.SearchMatchBackground != null)
            {
                var matches = GetOrCreateTable(search, "matches");
                if (palette.SearchMatchForeground != null) matches["foreground"] = palette.SearchMatchForeground;
                if (palette.SearchMatchBackground != null) matches["background"] = palette.SearchMatchBackground;
            }

            if (palette.SearchFocusedForeground != null || palette.SearchFocusedBackground != null)
            {
                var focused = GetOrCreateTable(search, "focused_match");
                if (palette.SearchFocusedForeground != null) focused["foreground"] = palette.SearchFocusedForeground;
                if (palette.SearchFocusedBackground != null) focused["background"] = palette.SearchFocusedBackground;
            }
        }

        if (palette.FooterBarForeground != null || palette.FooterBarBackground != null)
        {
            var footerBar = GetOrCreateTable(colors, "footer_bar");
            if (palette.FooterBarForeground != null) footerBar["foreground"] = palette.FooterBarForeground;
            if (palette.FooterBarBackground != null) footerBar["background"] = palette.FooterBarBackground;
        }

        if (palette.ViModeCursorText != null || palette.ViModeCursorColor != null)
        {
            var viModeCursor = GetOrCreateTable(colors, "vi_mode_cursor");
            if (palette.ViModeCursorText != null) viModeCursor["text"] = palette.ViModeCursorText;
            if (palette.ViModeCursorColor != null) viModeCursor["cursor"] = palette.ViModeCursorColor;
        }

        if (palette.HintsStartForeground != null || palette.HintsStartBackground != null ||
            palette.HintsEndForeground != null || palette.HintsEndBackground != null)
        {
            var hints = GetOrCreateTable(colors, "hints");

            if (palette.HintsStartForeground != null || palette.HintsStartBackground != null)
            {
                var start = GetOrCreateTable(hints, "start");
                if (palette.HintsStartForeground != null) start["foreground"] = palette.HintsStartForeground;
                if (palette.HintsStartBackground != null) start["background"] = palette.HintsStartBackground;
            }

            if (palette.HintsEndForeground != null || palette.HintsEndBackground != null)
            {
                var end = GetOrCreateTable(hints, "end");
                if (palette.HintsEndForeground != null) end["foreground"] = palette.HintsEndForeground;
                if (palette.HintsEndBackground != null) end["background"] = palette.HintsEndBackground;
            }
        }

        if (palette.LineIndicatorForeground != null || palette.LineIndicatorBackground != null)
        {
            var lineIndicator = GetOrCreateTable(colors, "line_indicator");
            if (palette.LineIndicatorForeground != null) lineIndicator["foreground"] = palette.LineIndicatorForeground;
            if (palette.LineIndicatorBackground != null) lineIndicator["background"] = palette.LineIndicatorBackground;
        }

        if (palette.DimBlack != null || palette.DimRed != null || palette.DimGreen != null ||
            palette.DimYellow != null || palette.DimBlue != null || palette.DimMagenta != null ||
            palette.DimCyan != null || palette.DimWhite != null)
        {
            var dim = GetOrCreateTable(colors, "dim");
            if (palette.DimBlack != null) dim["black"] = palette.DimBlack;
            if (palette.DimRed != null) dim["red"] = palette.DimRed;
            if (palette.DimGreen != null) dim["green"] = palette.DimGreen;
            if (palette.DimYellow != null) dim["yellow"] = palette.DimYellow;
            if (palette.DimBlue != null) dim["blue"] = palette.DimBlue;
            if (palette.DimMagenta != null) dim["magenta"] = palette.DimMagenta;
            if (palette.DimCyan != null) dim["cyan"] = palette.DimCyan;
            if (palette.DimWhite != null) dim["white"] = palette.DimWhite;
        }

        colors["draw_bold_text_with_bright_colors"] = palette.DrawBoldTextWithBrightColors;
        colors["transparent_background_colors"] = palette.TransparentBackgroundColors;
    }

    private static void WriteFont(TomlTable doc, FontConfig font)
    {
        var fontTable = GetOrCreateTable(doc, "font");
        fontTable["size"] = font.Size;
        fontTable["builtin_box_drawing"] = font.BuiltinBoxDrawing;

        var normal = GetOrCreateTable(fontTable, "normal");
        normal["family"] = font.NormalFamily;
        normal["style"] = font.NormalStyle;

        if (!string.IsNullOrEmpty(font.BoldFamily))
        {
            var bold = GetOrCreateTable(fontTable, "bold");
            bold["family"] = font.BoldFamily;
            bold["style"] = font.BoldStyle;
        }

        if (!string.IsNullOrEmpty(font.ItalicFamily))
        {
            var italic = GetOrCreateTable(fontTable, "italic");
            italic["family"] = font.ItalicFamily;
            italic["style"] = font.ItalicStyle;
        }

        if (!string.IsNullOrEmpty(font.BoldItalicFamily))
        {
            var boldItalic = GetOrCreateTable(fontTable, "bold_italic");
            boldItalic["family"] = font.BoldItalicFamily;
            boldItalic["style"] = font.BoldItalicStyle;
        }

        var offset = GetOrCreateTable(fontTable, "offset");
        offset["x"] = (long)font.OffsetX;
        offset["y"] = (long)font.OffsetY;

        var glyphOffset = GetOrCreateTable(fontTable, "glyph_offset");
        glyphOffset["x"] = (long)font.GlyphOffsetX;
        glyphOffset["y"] = (long)font.GlyphOffsetY;
    }

    private static void WriteWindow(TomlTable doc, WindowConfig window)
    {
        var windowTable = GetOrCreateTable(doc, "window");
        windowTable["decorations"] = window.Decorations;
        windowTable["dynamic_padding"] = window.DynamicPadding;
        windowTable["opacity"] = window.Opacity;
        windowTable["blur"] = window.Blur;
        windowTable["startup_mode"] = window.StartupMode;
        windowTable["title"] = window.Title;
        windowTable["dynamic_title"] = window.DynamicTitle;
        windowTable["resize_increments"] = window.ResizeIncrements;

        var dimensions = GetOrCreateTable(windowTable, "dimensions");
        dimensions["columns"] = (long)window.Columns;
        dimensions["lines"] = (long)window.Lines;

        var padding = GetOrCreateTable(windowTable, "padding");
        padding["x"] = (long)window.PaddingX;
        padding["y"] = (long)window.PaddingY;

        if (window.HasPosition)
        {
            var position = GetOrCreateTable(windowTable, "position");
            position["x"] = (long)window.PositionX;
            position["y"] = (long)window.PositionY;
        }

        // window.class is Linux/BSD only — skip on other platforms unless already present
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS()
            || windowTable.ContainsKey("class"))
        {
            var cls = GetOrCreateTable(windowTable, "class");
            cls["instance"] = window.ClassInstance;
            cls["general"] = window.ClassGeneral;
        }

        windowTable["decorations_theme_variant"] = window.DecorationsThemeVariant;

        // option_as_alt is macOS only — skip on other platforms unless already present
        if (OperatingSystem.IsMacOS() || windowTable.ContainsKey("option_as_alt"))
            windowTable["option_as_alt"] = window.OptionAsAlt;

        windowTable["level"] = window.Level;
    }

    private static void WriteCursor(TomlTable doc, CursorConfig cursor)
    {
        var cursorTable = GetOrCreateTable(doc, "cursor");

        if (cursor.ViModeEnabled)
        {
            var viMode = GetOrCreateTable(cursorTable, "vi_mode_style");
            viMode["shape"] = cursor.ViModeShape;
            viMode["blinking"] = cursor.ViModeBlinking;
        }
        else
        {
            cursorTable["vi_mode_style"] = "None";
        }
        cursorTable["blink_interval"] = (long)cursor.BlinkInterval;
        cursorTable["blink_timeout"] = (long)cursor.BlinkTimeout;
        cursorTable["unfocused_hollow"] = cursor.UnfocusedHollow;
        cursorTable["thickness"] = cursor.Thickness;

        var style = GetOrCreateTable(cursorTable, "style");
        style["shape"] = cursor.Shape;
        style["blinking"] = cursor.Blinking;
    }

    private static void WriteTerminal(TomlTable doc, TerminalConfig terminal)
    {
        var scrolling = GetOrCreateTable(doc, "scrolling");
        scrolling["history"] = (long)terminal.ScrollingHistory;
        scrolling["multiplier"] = (long)terminal.ScrollingMultiplier;

        var terminalTable = GetOrCreateTable(doc, "terminal");
        terminalTable["osc52"] = terminal.Osc52;

        if (!string.IsNullOrEmpty(terminal.ShellProgram))
        {
            var shell = GetOrCreateTable(terminalTable, "shell");
            shell["program"] = terminal.ShellProgram;

            if (terminal.ShellArgs.Count > 0)
            {
                var argsArr = new TomlArray();
                foreach (var arg in terminal.ShellArgs)
                    argsArr.Add(arg);
                shell["args"] = argsArr;
            }
            else
            {
                shell.Remove("args");
            }
        }

        var selection = GetOrCreateTable(doc, "selection");
        selection["save_to_clipboard"] = terminal.SaveToClipboard;
        selection["semantic_escape_chars"] = terminal.SemanticEscapeChars;

        var mouse = GetOrCreateTable(doc, "mouse");
        mouse["hide_when_typing"] = terminal.HideMouseWhenTyping;

        var general = GetOrCreateTable(doc, "general");
        general["live_config_reload"] = terminal.LiveConfigReload;
        general["ipc_socket"] = terminal.IpcSocket;

        if (!string.IsNullOrEmpty(terminal.WorkingDirectory))
            general["working_directory"] = terminal.WorkingDirectory;

        if (terminal.Import.Count > 0)
        {
            var importArr = new TomlArray();
            foreach (var path in terminal.Import)
                importArr.Add(path);
            general["import"] = importArr;
        }

        var bell = GetOrCreateTable(doc, "bell");
        bell["animation"] = terminal.BellAnimation;
        bell["duration"] = (long)terminal.BellDuration;
        bell["color"] = terminal.BellColor;

        if (!string.IsNullOrEmpty(terminal.BellCommand))
            bell["command"] = WriteCommand(terminal.BellCommand, terminal.BellCommandArgs);
    }

    private static void WriteHints(TomlTable doc, HintsConfig hints)
    {
        var hintsTable = GetOrCreateTable(doc, "hints");
        hintsTable["alphabet"] = hints.Alphabet;

        if (hints.Enabled.Count > 0)
        {
            var enabledArr = new TomlTableArray();
            foreach (var rule in hints.Enabled)
            {
                var ruleTable = new TomlTable();
                if (!string.IsNullOrEmpty(rule.Regex))
                    ruleTable["regex"] = rule.Regex;
                if (rule.Hyperlinks)
                    ruleTable["hyperlinks"] = true;
                if (rule.PostProcessing)
                    ruleTable["post_processing"] = true;
                if (rule.Persist)
                    ruleTable["persist"] = true;
                if (!string.IsNullOrEmpty(rule.Action))
                    ruleTable["action"] = rule.Action;
                if (!string.IsNullOrEmpty(rule.Command))
                    ruleTable["command"] = WriteCommand(rule.Command, rule.CommandArgs);
                if (!string.IsNullOrEmpty(rule.BindingKey))
                {
                    var binding = new TomlTable();
                    binding["key"] = rule.BindingKey;
                    if (!string.IsNullOrEmpty(rule.BindingMods))
                        binding["mods"] = rule.BindingMods;
                    ruleTable["binding"] = binding;
                }
                if (!string.IsNullOrEmpty(rule.MouseMods) || !rule.MouseEnabled)
                {
                    var mouse = new TomlTable();
                    mouse["enabled"] = rule.MouseEnabled;
                    if (!string.IsNullOrEmpty(rule.MouseMods))
                        mouse["mods"] = rule.MouseMods;
                    ruleTable["mouse"] = mouse;
                }
                enabledArr.Add(ruleTable);
            }
            hintsTable["enabled"] = enabledArr;
        }
    }

    private static void WriteKeyboard(TomlTable doc, KeyboardConfig keyboard)
    {
        if (keyboard.Bindings.Count == 0) return;

        var kbTable = GetOrCreateTable(doc, "keyboard");
        var bindingsArr = new TomlTableArray();
        foreach (var b in keyboard.Bindings)
        {
            var bt = new TomlTable();
            bt["key"] = b.Key;
            if (!string.IsNullOrEmpty(b.Mods)) bt["mods"] = b.Mods;
            if (!string.IsNullOrEmpty(b.Mode)) bt["mode"] = b.Mode;
            if (!string.IsNullOrEmpty(b.Action)) bt["action"] = b.Action;
            if (!string.IsNullOrEmpty(b.Command)) bt["command"] = WriteCommand(b.Command, b.CommandArgs);
            if (!string.IsNullOrEmpty(b.Chars)) bt["chars"] = b.Chars;
            bindingsArr.Add(bt);
        }
        kbTable["bindings"] = bindingsArr;
    }

    private static void WriteDebug(TomlTable doc, DebugConfig debug)
    {
        var debugTable = GetOrCreateTable(doc, "debug");
        debugTable["render_timer"] = debug.RenderTimer;
        debugTable["persistent_logging"] = debug.PersistentLogging;
        debugTable["log_level"] = debug.LogLevel;
        debugTable["renderer"] = debug.Renderer;
        debugTable["print_events"] = debug.PrintEvents;
        debugTable["highlight_damage"] = debug.HighlightDamage;
        debugTable["prefer_egl"] = debug.PreferEgl;
    }

    private static object WriteCommand(string program, List<string> args)
    {
        // simple string command when no args
        if (args.Count == 0)
            return program;

        // table format: { program = "...", args = ["..."] }
        var table = new TomlTable();
        table["program"] = program;
        var argsArr = new TomlArray();
        foreach (var arg in args)
            argsArr.Add(arg);
        table["args"] = argsArr;
        return table;
    }

    private static TomlTable GetOrCreateTable(TomlTable parent, string key)
    {
        if (!parent.ContainsKey(key) || parent[key] is not TomlTable)
            parent[key] = new TomlTable();
        return (TomlTable)parent[key];
    }

    private static void CreateBackup(string path)
    {
        if (!File.Exists(path)) return;

        var backupPath = GetNextBackupPath(path);
        try
        {
            File.Copy(path, backupPath, overwrite: false);
            Logger.Information("Backup created at {BackupPath}", backupPath);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to create backup at {BackupPath}", backupPath);
            throw;
        }
    }

    private static string GetNextBackupPath(string path)
    {
        // first backup is .bak, then .bak2, .bak3, ...
        var firstBackup = path + ".bak";
        if (!File.Exists(firstBackup))
            return firstBackup;

        var dir = Path.GetDirectoryName(path) ?? ".";
        var fileName = Path.GetFileName(path);
        var prefix = fileName + ".bak";

        int highest = 1;
        try
        {
            foreach (var file in Directory.GetFiles(dir, prefix + "*"))
            {
                var name = Path.GetFileName(file);
                var suffix = name[prefix.Length..];

                if (suffix.Length == 0)
                    continue; // that's .bak itself, already counted as 1

                if (int.TryParse(suffix, out var num) && num > highest)
                    highest = num;
            }
        }
        catch (Exception ex)
        {
            Logger.Warning(ex, "Could not enumerate backup files in {Dir}, falling back to next index", dir);
        }

        return path + ".bak" + (highest + 1);
    }
}
