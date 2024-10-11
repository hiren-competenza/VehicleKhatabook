using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IBackupRepository
    {
        Task<Backup> BackupDataAsync(Backup backup);
        Task<Backup?> RestoreDataAsync(Guid userId, Guid backupId);
    }
}
