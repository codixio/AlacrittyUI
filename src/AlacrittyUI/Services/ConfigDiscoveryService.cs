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

        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(home, ".config", "alacritty", "alacritty.toml");
    }

    private static List<string> GetCandidatePaths()
    {
        var paths = new List<string>();
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (OperatingSystem.IsWindows())
        {
            // official Windows path
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            paths.Add(Path.Combine(appData, "alacritty", "alacritty.toml"));

            // many users use ~/.config/ on Windows too (e.g. via scoop, msys2, or manually)
            paths.Add(Path.Combine(home, ".config", "alacritty", "alacritty.toml"));
        }
        else
        {
            var xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
            if (!string.IsNullOrEmpty(xdgConfig))
                paths.Add(Path.Combine(xdgConfig, "alacritty", "alacritty.toml"));

            paths.Add(Path.Combine(home, ".config", "alacritty", "alacritty.toml"));
        }

        return paths;
    }
}
