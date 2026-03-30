using Serilog;

namespace AlacrittyUI.Services;

public class ConfigDiscoveryService
{
    private static readonly ILogger Logger = Log.ForContext<ConfigDiscoveryService>();

    public string? FindConfigPath()
    {
        var candidates = GetCandidatePaths();

        foreach (var path in candidates)
        {
            if (File.Exists(path))
            {
                Logger.Information("Found Alacritty config at {Path}", path);
                return path;
            }
        }

        Logger.Warning("No Alacritty config found in standard locations");
        return null;
    }

    public static string GetDefaultConfigPath()
    {
        if (OperatingSystem.IsWindows())
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "alacritty", "alacritty.toml");
        }

        // respect XDG_CONFIG_HOME on Linux/macOS, fall back to ~/.config
        var xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
        if (!string.IsNullOrEmpty(xdgConfig))
            return Path.Combine(xdgConfig, "alacritty", "alacritty.toml");

        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(home, ".config", "alacritty", "alacritty.toml");
    }

    private static List<string> GetCandidatePaths()
    {
        var paths = new List<string>();

        if (OperatingSystem.IsWindows())
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            paths.Add(Path.Combine(appData, "alacritty", "alacritty.toml"));
        }
        else
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
            if (!string.IsNullOrEmpty(xdgConfig))
                paths.Add(Path.Combine(xdgConfig, "alacritty", "alacritty.toml"));

            paths.Add(Path.Combine(home, ".config", "alacritty", "alacritty.toml"));
        }

        return paths;
    }
}
