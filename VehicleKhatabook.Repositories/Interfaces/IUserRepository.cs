using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(UserDTO userDTO);
        Task<User> GetUserByIdAsync(Guid id); 
        Task<User?> GetUserByMobileAsync(string mobileNumber);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddDriverAsync(UserDTO userDTO);
        Task<User> GetDriverByIdAsync(Guid id);
        Task<User> UpdateDriverAsync(Guid id, UserDTO userDTO);
        Task<bool> DeleteDriverAsync(Guid id);
        Task<List<User>> GetAllDriversAsync();
        Task<UserDetailsDTO> AuthenticateUser(UserLoginDTO userLoginDTO);
        Task<UserDetailsDTO> GetUserDetailsbyMobileAsync(string mobileNumber);
        Task<bool> UpdateUserRoleAsync(Guid userId, string role);
        Task<bool> UpdateUserLanguageAsync(Guid userId, int languageTypeId);
    }
}
