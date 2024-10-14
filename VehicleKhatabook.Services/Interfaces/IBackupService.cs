using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IBackupService
    {
        Task<BackupDTO> BackupDataAsync(Guid userId, byte[] data);
        Task<BackupDTO?> RestoreDataAsync(Guid userId, Guid backupId);
    }
}
