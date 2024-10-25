using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IAdminUserService
    {
        Task<AdminUser> RegisterAdminAsync(AdminUserDTO adminUserDTO);
        Task<AdminUser> UpdateAdminAsync(AdminUserDTO adminUserDTO);
        Task<IEnumerable<AdminUser>> GetAllAdminsAsync();
        Task<AdminUser> GetAdminByIdAsync(int adminId);
    }

}
