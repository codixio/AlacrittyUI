using System.Text.Json;
using Serilog;

namespace AlacrittyUI.Services;

public class AppSettings
{
    public double WindowWidth { get; set; } = 1350;
    public double WindowHeight { get; set; } = 900;
    public double? WindowX { get; set; }
    public double? WindowY { get; set; }
    public double UiScale { get; set; } = 1.0;
    public string? LastConfigPath { get; set; }
}

public class AppSettingsService
{
    private static readonly ILogger Logger = Log.ForContext<AppSettingsService>();
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _settingsPath;

    public AppSettings Settings { get; private set; } = new();

    public AppSettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "AlacrittyUI");
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
