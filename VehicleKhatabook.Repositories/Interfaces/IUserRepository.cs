using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserDTO userDTO);
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task UpdateUserAsync(Guid id, UserDTO userDTO);
        Task<bool> DeleteUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<User> GetUserByMobileNumberAsync(string mobileNumber);
    }
}
