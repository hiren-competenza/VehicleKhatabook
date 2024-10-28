using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<AdminUser> RegisterAdminAsync(AdminUser adminUser);
        Task<AdminUser> UpdateAdminAsync(AdminUser adminUser);
        Task<IEnumerable<AdminUser>> GetAllAdminsAsync();
        Task<AdminUser> GetAdminByIdAsync(int adminId);
    }

}
