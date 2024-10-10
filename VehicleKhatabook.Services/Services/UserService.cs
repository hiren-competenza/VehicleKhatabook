using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            await _userRepository.AddUserAsync(userDTO);
            return userDTO;
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<UserDTO?> UpdateUserAsync(Guid id, UserDTO userDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            await _userRepository.UpdateUserAsync(id, userDTO);
            return userDTO;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
