using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserDTO userDTO);
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<User> UpdateUserAsync(Guid id, UserDTO userDTO);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<User> AddDriverAsync(UserDTO driverDTO);
        Task<User> GetDriverByIdAsync(Guid id);
        Task<User> UpdateDriverAsync(Guid id, UserDTO driverDTO);
        Task<bool> DeleteDriverAsync(Guid id);
        Task<List<User>> GetAllDriversAsync();
    }
}
