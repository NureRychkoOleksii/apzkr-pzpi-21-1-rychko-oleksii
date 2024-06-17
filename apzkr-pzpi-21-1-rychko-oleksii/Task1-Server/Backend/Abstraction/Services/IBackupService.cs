namespace Backend.Abstraction.Services;

public interface IBackupService
{
    Task SaveToCsv(string directoryPath);
    Task RestoreFromCsv(string directoryPath);
    Task<byte[]> DownloadAsZip(string directoryPath);
}