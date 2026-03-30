using System.Text.Json;
using Serilog;

namespace AlacrittyUI.Services;

public class AppSettings
{
    public const double DefaultWindowWidth = 1350;
    public const double DefaultWindowHeight = 900;
    public const double DefaultUiScale = 1.0;

    public double WindowWidth { get; set; } = DefaultWindowWidth;
    public double WindowHeight { get; set; } = DefaultWindowHeight;
    public double? WindowX { get; set; }
    public double? WindowY { get; set; }
    public double UiScale { get; set; } = DefaultUiScale;
    public string? LastConfigPath { get; set; }
    public string Language { get; set; } = "en";
}

public class AppSettingsService
{
    private static readonly ILogger Logger = Log.ForContext<AppSettingsService>();
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _settingsPath;

    public AppSettings Settings { get; private set; } = new();

    public AppSettingsService()
    {
        string dir;
        if (OperatingSystem.IsWindows())
        {
            dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AlacrittyUI");
        }
        else
        {
            // Linux/macOS: prefer $XDG_CONFIG_HOME, fall back to ~/.config
            var xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
            dir = !string.IsNullOrEmpty(xdgConfig)
                ? Path.Combine(xdgConfig, "AlacrittyUI")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    ".config", "AlacrittyUI");
        }

        _settingsPath = Path.Combine(dir, "settings.json");
    }

    public void Load()
    {
        if (!File.Exists(_settingsPath))
        {
            Logger.Information("No app settings found, using defaults");
            return;
        }

        try
        {
            var json = File.ReadAllText(_settingsPath);
            Settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? new AppSettings();
            Logger.Information("App settings loaded from {Path}", _settingsPath);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to load app settings from {Path}", _settingsPath);
            Settings = new AppSettings();
        }
    }

    public void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(_settingsPath)!;
            Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(Settings, JsonOptions);
            File.WriteAllText(_settingsPath, json);
            Logger.Information("App settings saved to {Path}", _settingsPath);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to save app settings to {Path}", _settingsPath);
        }
    }
}
