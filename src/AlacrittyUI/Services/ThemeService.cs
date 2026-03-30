using System.Reflection;
using System.Text;
using AlacrittyUI.Models;
using Serilog;

namespace AlacrittyUI.Services;

public class ThemeService
{
    private static readonly ILogger Logger = Log.ForContext<ThemeService>();
    private const string BuiltInPrefix = "AlacrittyUI.Assets.BuiltInThemes.";

    private readonly ConfigReaderService _reader;
    private readonly ConfigWriterService _writer;

    public ThemeService(ConfigReaderService reader, ConfigWriterService writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public List<ThemeInfo> GetBuiltInThemes()
    {
        var themes = new List<ThemeInfo>();
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var resourceName in assembly.GetManifestResourceNames()
                     .Where(n => n.StartsWith(BuiltInPrefix) && n.EndsWith(".toml"))
                     .OrderBy(n => n))
        {
            var fileName = resourceName[BuiltInPrefix.Length..^5];
            var displayName = FormatThemeName(fileName);

            // read preview colors
            string? bg = null, fg = null;
            try
            {
                var palette = LoadBuiltInPalette(resourceName);
                bg = palette.Background;
                fg = palette.Foreground;
            }
            catch (Exception ex)
            {
                Logger.Warning(ex, "Failed to read preview for built-in theme {Name}", displayName);
            }

            themes.Add(new ThemeInfo
            {
                Name = displayName,
                FilePath = resourceName,
                IsBuiltIn = true,
                PreviewBackground = bg,
                PreviewForeground = fg
            });
        }

        return themes;
    }

    public List<ThemeInfo> GetUserThemes()
    {
        var dir = GetUserThemesDirectory();
        if (!Directory.Exists(dir))
            return [];

        return Directory.GetFiles(dir, "*.toml")
            .Select(path =>
            {
                string? bg = null, fg = null;
                try
                {
                    var config = _reader.ReadFromFile(path);
                    bg = config.Colors.Background;
                    fg = config.Colors.Foreground;
                }
                catch (Exception ex)
                {
                    Logger.Warning(ex, "Failed to read user theme {Path}", path);
                }

                return new ThemeInfo
                {
                    Name = Path.GetFileNameWithoutExtension(path),
                    FilePath = path,
                    IsBuiltIn = false,
                    PreviewBackground = bg,
                    PreviewForeground = fg
                };
            })
            .OrderBy(t => t.Name)
            .ToList();
    }

    public ColorPalette LoadPalette(ThemeInfo theme)
    {
        if (theme.IsBuiltIn)
            return LoadBuiltInPalette(theme.FilePath);

        return _reader.ReadFromFile(theme.FilePath).Colors;
    }

    public void SaveTheme(string name, ColorPalette palette)
    {
        var dir = GetUserThemesDirectory();
        Directory.CreateDirectory(dir);

        var path = Path.Combine(dir, $"{name}.toml");
        var config = new AlacrittyConfig { Colors = palette };
        _writer.WriteConfig(path, config);
        Logger.Information("Theme saved as {Name} at {Path}", name, path);
    }

    public void DeleteUserTheme(ThemeInfo theme)
    {
        if (theme.IsBuiltIn) return;

        try
        {
            if (File.Exists(theme.FilePath))
            {
                File.Delete(theme.FilePath);
                Logger.Information("Deleted user theme {Name}", theme.Name);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to delete theme {Name} at {Path}", theme.Name, theme.FilePath);
            throw;
        }
    }

    public void ExportTheme(string targetPath, ColorPalette palette)
    {
        var config = new AlacrittyConfig { Colors = palette };
        _writer.WriteConfig(targetPath, config);
        Logger.Information("Theme exported to {Path}", targetPath);
    }

    public ColorPalette ImportTheme(string sourcePath)
    {
        Logger.Information("Importing theme from {Path}", sourcePath);
        return _reader.ReadFromFile(sourcePath).Colors;
    }

    private ColorPalette LoadBuiltInPalette(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
                           ?? throw new FileNotFoundException($"Built-in theme resource not found: {resourceName}");
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var toml = reader.ReadToEnd();
        return _reader.ReadFromString(toml).Colors;
    }

    private static string GetUserThemesDirectory()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, "AlacrittyUI", "themes");
    }

    private static string FormatThemeName(string fileName)
    {
        return string.Join(" ", fileName
            .Replace('-', ' ')
            .Replace('_', ' ')
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => char.ToUpperInvariant(w[0]) + w[1..]));
    }
}
