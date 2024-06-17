using Backend.Abstraction.Services;
using Backend.Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BackupController : ControllerBase
{
    private readonly IBackupService _backupService;

    public BackupController(IBackupService backupService)
    {
        _backupService = backupService;
    }

    [HttpPost("save")]
    [AdminRoleInterceptor]
    public async Task<IActionResult> SaveToCsv()
    {
        string relativeDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backups");
        Console.WriteLine(relativeDirectoryPath);
        Directory.CreateDirectory(relativeDirectoryPath);

        await _backupService.SaveToCsv(relativeDirectoryPath);

        return Ok("Database saved to CSV.");
    }

    [HttpPost("restore")]
    [AdminRoleInterceptor]
    public async Task<IActionResult> RestoreFromCsv()
    {
        string relativeDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backups");
        Console.WriteLine(relativeDirectoryPath);
        
        if (!Directory.Exists(relativeDirectoryPath))
        {
            return BadRequest("Backup directory does not exist.");
        }

        await _backupService.RestoreFromCsv(relativeDirectoryPath);
        return Ok("Database restored from CSV.");
    }

    [HttpPost("download")]
    [AdminRoleInterceptor]
    public async Task<IActionResult> DownloadAsZip()
    {
        string relativeDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backups");
        Directory.CreateDirectory(relativeDirectoryPath);

        var zipFileData = await _backupService.DownloadAsZip(relativeDirectoryPath);

        return File(zipFileData, "application/zip", "database_backup.zip");
    }
}