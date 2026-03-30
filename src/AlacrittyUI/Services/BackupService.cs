using System.Text;
using AlacrittyUI.Models;
using Serilog;

namespace AlacrittyUI.Services;

public class BackupService
{
    private static readonly ILogger Logger = Log.ForContext<BackupService>();

    public List<BackupInfo> GetBackups(string configPath)
    {
        var dir = Path.GetDirectoryName(configPath);
        if (dir == null || !Directory.Exists(dir))
            return [];

        var fileName = Path.GetFileName(configPath);
        var prefix = fileName + ".bak";

        try
        {
            return Directory.GetFiles(dir, prefix + "*")
                .Select(path => new FileInfo(path))
                .OrderByDescending(fi => fi.LastWriteTime)
                .Select(fi => new BackupInfo
                {
                    FilePath = fi.FullName,
                    FileName = fi.Name,
                    Modified = fi.LastWriteTime,
                    SizeBytes = fi.Length
                })
                .ToList();
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to enumerate backups in {Dir}", dir);
            return [];
        }
    }

    public string ReadBackupContent(string backupPath)
    {
        try
        {
            return File.ReadAllText(backupPath, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to read backup {Path}", backupPath);
            return $"Error reading backup: {ex.Message}";
        }
    }

    public void RestoreBackup(string backupPath, string configPath)
    {
        Logger.Information("Restoring backup {Backup} to {Config}", backupPath, configPath);

        // safety net: back up current config before overwriting
        if (File.Exists(configPath))
        {
            var safetyBackup = configPath + ".pre-restore";
            File.Copy(configPath, safetyBackup, overwrite: true);
            Logger.Information("Safety backup created at {Path}", safetyBackup);
        }

        File.Copy(backupPath, configPath, overwrite: true);
        Logger.Information("Backup restored successfully");
    }
}
