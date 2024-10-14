using AutoMapper;
using Microsoft.Extensions.Logging;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class BackupService : IBackupService
    {
        private readonly IBackupRepository _backupRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BackupService> _logger;

        public BackupService(IBackupRepository backupRepository, IMapper mapper, ILogger<BackupService> logger)
        {
            _backupRepository = backupRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BackupDTO> BackupDataAsync(Guid userId, byte[] data)
        {
            try
            {
                _logger.LogInformation("Starting backup for user {UserId}", userId);

                var backup = new Backup
                {
                    UserID = userId,
                    BackupData = data,
                };

                var createdBackup = await _backupRepository.BackupDataAsync(backup);

                _logger.LogInformation("Backup created successfully for user {UserId} with backup ID {BackupId}", userId, createdBackup.BackupID);

                return _mapper.Map<BackupDTO>(createdBackup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while backing up data for user {UserId}", userId);
                throw;
            }
        }

        public async Task<BackupDTO?> RestoreDataAsync(Guid userId, Guid backupId)
        {
            try
            {
                _logger.LogInformation("Attempting to restore backup {BackupId} for user {UserId}", backupId, userId);

                var backup = await _backupRepository.RestoreDataAsync(userId, backupId);

                if (backup == null)
                {
                    _logger.LogWarning("No backup found for user {UserId} with backup ID {BackupId}", userId, backupId);
                    return null;
                }

                _logger.LogInformation("Backup {BackupId} restored successfully for user {UserId}", backupId, userId);
                return _mapper.Map<BackupDTO>(backup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while restoring backup {BackupId} for user {UserId}", backupId, userId);
                throw;
            }
        }
    }
}
