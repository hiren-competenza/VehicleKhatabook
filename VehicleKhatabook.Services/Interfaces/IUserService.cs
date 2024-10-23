using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<User>> CreateUserAsync(UserDTO userDTO);
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<ApiResponse<User>> UpdateUserAsync(Guid id, UserDTO userDTO);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<ApiResponse<User>> AddDriverAsync(UserDTO driverDTO);
        Task<ApiResponse<User?>> GetDriverByIdAsync(Guid id);
        Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO driverDTO);
        Task<ApiResponse<bool>> DeleteDriverAsync(Guid id);
        Task<ApiResponse<List<User>>> GetAllDriversAsync();
    }
}
