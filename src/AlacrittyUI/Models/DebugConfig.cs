namespace AlacrittyUI.Models;

public class DebugConfig
{
    public bool RenderTimer { get; set; }
    public bool PersistentLogging { get; set; }
    public string LogLevel { get; set; } = "Warn";
    public string Renderer { get; set; } = "None";
    public bool PrintEvents { get; set; }
    public bool HighlightDamage { get; set; }
    public bool PreferEgl { get; set; }

    public static readonly string[] LogLevelOptions = ["Off", "Error", "Warn", "Info", "Debug", "Trace"];
    public static readonly string[] RendererOptions = ["None", "glsl3", "gles2", "gles2pure"];
}
