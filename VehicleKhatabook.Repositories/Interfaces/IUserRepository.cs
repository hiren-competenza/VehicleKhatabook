using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(UserDTO userDTO);
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<User> UpdateUserAsync(UserDTO userDTO);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<User> GetUserByMobileNumberAsync(string mobileNumber);
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddDriverAsync(UserDTO userDTO);
        Task<User> GetDriverByIdAsync(Guid id);
        Task<User> UpdateDriverAsync(Guid id, UserDTO userDTO);
        Task<bool> DeleteDriverAsync(Guid id);
        Task<List<User>> GetAllDriversAsync();
        Task<UserDetailsDTO> AuthenticateUser(UserLoginDTO userLoginDTO);
    }
}
