using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<AdminUserDTO> GetAdminByMobileAndPasswordAsync(string mobileNumber, string password);
        Task<AdminUser> RegisterAdminAsync(AdminUser adminUser);
        Task<AdminUser> UpdateAdminAsync(AdminUser adminUser);
        Task<IEnumerable<AdminUser>> GetAllAdminsAsync();
        Task<AdminUser> GetAdminByIdAsync(int adminId);
    }

}
