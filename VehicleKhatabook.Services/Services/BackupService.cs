using AutoMapper;
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

        public BackupService(IBackupRepository backupRepository, IMapper mapper)
        {
            _backupRepository = backupRepository;
            _mapper = mapper;
        }

        public async Task<BackupDTO> BackupDataAsync(Guid userId, byte[] data)
        {
            var backup = new Backup
            {
                UserID = userId,
                BackupData = data,
                //CreatedBy = userId
            };

            var createdBackup = await _backupRepository.BackupDataAsync(backup);
            return _mapper.Map<BackupDTO>(createdBackup);
        }

        public async Task<BackupDTO?> RestoreDataAsync(Guid userId, Guid backupId)
        {
            var backup = await _backupRepository.RestoreDataAsync(userId, backupId);
            return backup != null ? _mapper.Map<BackupDTO>(backup) : null;
        }
    }
}
