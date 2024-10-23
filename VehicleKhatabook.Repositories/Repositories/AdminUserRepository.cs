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

        public async Task<ApiResponse<AdminUser>> RegisterAdminAsync(AdminUser adminUser)
        {
            _context.AdminUsers.Add(adminUser);
            await _context.SaveChangesAsync();
            return ApiResponse<AdminUser>.SuccessResponse(adminUser);
        }

        public async Task<ApiResponse<AdminUser>> UpdateAdminAsync(AdminUser adminUser)
        {
            var existingAdmin = await _context.AdminUsers.FindAsync(adminUser.AdminID);
            if (existingAdmin == null)
            {
                return ApiResponse<AdminUser>.FailureResponse("Admin not found.");
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
            return ApiResponse<AdminUser>.SuccessResponse(existingAdmin);
        }

        public async Task<ApiResponse<IEnumerable<AdminUser>>> GetAllAdminsAsync()
        {
            var adminUsers = await _context.AdminUsers.ToListAsync();
            return ApiResponse<IEnumerable<AdminUser>>.SuccessResponse(adminUsers);
        }

        public async Task<ApiResponse<AdminUser>> GetAdminByIdAsync(int adminId)
        {
            var adminUser = await _context.AdminUsers.FindAsync(adminId);
            if (adminUser == null)
            {
                return ApiResponse<AdminUser>.FailureResponse("Admin not found.");
            }
            return ApiResponse<AdminUser>.SuccessResponse(adminUser);
        }
    }

}
