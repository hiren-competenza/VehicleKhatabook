using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IDriverService
    {
        Task<ApiResponse<User>> AddDriverAsync(UserDTO driverDTO);
        Task<User?> GetDriverByIdAsync(Guid id);
        Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO driverDTO);
        Task<ApiResponse<bool>> DeleteDriverAsync(Guid id);
        Task<IEnumerable<User>> GetAllDriversAsync();
    }
}
