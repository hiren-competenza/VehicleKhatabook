using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IAdminUserService
    {
        Task<ApiResponse<AdminUser>> RegisterAdminAsync(AdminUserDTO adminUserDTO);
        Task<ApiResponse<AdminUser>> UpdateAdminAsync(AdminUserDTO adminUserDTO);
        Task<ApiResponse<IEnumerable<AdminUser>>> GetAllAdminsAsync();
        Task<ApiResponse<AdminUser>> GetAdminByIdAsync(int adminId);
    }

}
