using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserDTO userDTO);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User> UpdateUserAsync(UserDTO userDTO);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<bool> UpdateUserRoleAsync(Guid userId, string role);
        Task<bool> UpdateUserLanguageAsync(Guid userId, int languageTypeId);
    }
}
