using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApiResponse<User>> AddUserAsync(UserDTO userDTO);
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task UpdateUserAsync(Guid id, UserDTO userDTO);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<User> GetUserByMobileNumberAsync(string mobileNumber);
        Task<User> GetUserByIdAsync(int id);
        Task<ApiResponse<User>> AddDriverAsync(UserDTO userDTO);
        Task<ApiResponse<User?>> GetDriverByIdAsync(Guid id);
        Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO userDTO);
        Task<ApiResponse<bool>> DeleteDriverAsync(Guid id);
        Task<ApiResponse<List<User>>> GetAllDriversAsync();
        Task<ApiResponse<UserDetailsDTO>> AuthenticateUser(UserLoginDTO userLoginDTO);
    }
}
