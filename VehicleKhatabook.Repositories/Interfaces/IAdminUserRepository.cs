using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<ApiResponse<AdminUser>> RegisterAdminAsync(AdminUser adminUser);
        Task<ApiResponse<AdminUser>> UpdateAdminAsync(AdminUser adminUser);
        Task<ApiResponse<IEnumerable<AdminUser>>> GetAllAdminsAsync();
        Task<ApiResponse<AdminUser>> GetAdminByIdAsync(int adminId);
    }

}
