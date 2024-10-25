using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public AdminUserRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<AdminUser> RegisterAdminAsync(AdminUser adminUser)
        {
            _context.AdminUsers.Add(adminUser);
            await _context.SaveChangesAsync();
            return adminUser;
        }

        public async Task<AdminUser> UpdateAdminAsync(AdminUser adminUser)
        {
            var existingAdmin = await _context.AdminUsers.FindAsync(adminUser.AdminID);
            if (existingAdmin == null)
            {
                return null;
            }

            // Update fields as needed
            existingAdmin.FullName = adminUser.FullName;
            existingAdmin.Email = adminUser.Email;
            existingAdmin.MobileNumber = adminUser.MobileNumber;
            existingAdmin.Role = adminUser.Role;
            existingAdmin.IsActive = adminUser.IsActive;
            existingAdmin.ModifiedBy = adminUser.ModifiedBy;
            existingAdmin.LastModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingAdmin;
        }

        public async Task<IEnumerable<AdminUser>> GetAllAdminsAsync()
        {
            return await _context.AdminUsers.ToListAsync();
        }

        public async Task<AdminUser> GetAdminByIdAsync(int adminId)
        {
            return await _context.AdminUsers.FindAsync(adminId);
        }
    }

}
