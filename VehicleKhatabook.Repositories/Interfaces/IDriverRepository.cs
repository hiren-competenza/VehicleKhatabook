using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        Task<ApiResponse<User>> AddDriverAsync(UserDTO userDTO);
        Task<User?> GetDriverByIdAsync(Guid id);
        Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO userDTO);
        Task<ApiResponse<bool>> DeleteDriverAsync(Guid id);
        Task<IEnumerable<User>> GetAllDriversAsync();
    }
}
