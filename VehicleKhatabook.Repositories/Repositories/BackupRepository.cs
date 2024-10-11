using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class BackupRepository : IBackupRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public BackupRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<Backup> BackupDataAsync(Backup backup)
        {
            await _context.Backups.AddAsync(backup);
            await _context.SaveChangesAsync();
            return backup;
        }

        public async Task<Backup?> RestoreDataAsync(Guid userId, Guid backupId)
        {
            return await _context.Backups
                .FirstOrDefaultAsync(b => b.UserID == userId && b.BackupID == backupId);
        }
    }
}
