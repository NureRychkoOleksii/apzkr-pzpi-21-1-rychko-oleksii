using System.Collections;
using System.Globalization;
using Backend.Abstraction.Services;
using Backend.Core;
using CsvHelper;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class BackupService: IBackupService
{
    private readonly StarOfLifeContext _context;

    public BackupService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task SaveToCsv(string directoryPath)
    {
        Directory.CreateDirectory(directoryPath);
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var entityName = entityType.ClrType.Name;
            var filePath = Path.Combine(directoryPath, $"{entityName}.csv");
            
            var dbSetProperty = _context.GetType().GetProperty(entityName + "s");
            if (dbSetProperty == null) continue;
            
            var dbSet = dbSetProperty.GetValue(_context);
            var queryable = dbSet as IQueryable<object>;
            if (queryable == null) return;

            var entities = await queryable.ToListAsync();

            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(entities);
        }
    }

    public async Task RestoreFromCsv(string directoryPath)
    {
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var entityName = entityType.ClrType.Name;
            var filePath = Path.Combine(directoryPath, $"{entityName}.csv");

            if (!File.Exists(filePath)) continue;

            var dbSetProperty = _context.GetType().GetProperty(entityName + "s");
            if (dbSetProperty == null) continue;

            var dbSet = dbSetProperty.GetValue(_context);
            
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            
            var entityListType = typeof(List<>).MakeGenericType(entityType.ClrType);
            var entities = (IList)Activator.CreateInstance(entityListType);

            var records = csv.GetRecords(entityType.ClrType);
            foreach (var record in records)
            {
                entities.Add(record);
            }
            
            var addRangeMethod = dbSet.GetType().GetMethod("AddRange", new[] { typeof(IEnumerable<>).MakeGenericType(entityType.ClrType) });
            if (addRangeMethod != null)
            {
                addRangeMethod.Invoke(dbSet, new object[] { entities });
            }
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task<byte[]> DownloadAsZip(string directoryPath)
    {
        await SaveToCsv(directoryPath);

        using var memoryStream = new MemoryStream();
        using (var zipOutputStream = new ZipOutputStream(memoryStream))
        {
            zipOutputStream.SetLevel(3);

            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");
            foreach (var filePath in csvFiles)
            {
                var entry = new ZipEntry(Path.GetFileName(filePath))
                {
                    DateTime = DateTime.Now,
                    IsUnicodeText = true
                };
                zipOutputStream.PutNextEntry(entry);

                using var fileStream = File.OpenRead(filePath);
                fileStream.CopyTo(zipOutputStream);
                zipOutputStream.CloseEntry();
            }
        }

        return memoryStream.ToArray();
    }
}