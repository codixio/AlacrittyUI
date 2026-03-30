namespace AlacrittyUI.Models;

public class BackupInfo
{
    public required string FilePath { get; init; }
    public required string FileName { get; init; }
    public required DateTime Modified { get; init; }
    public required long SizeBytes { get; init; }
}
