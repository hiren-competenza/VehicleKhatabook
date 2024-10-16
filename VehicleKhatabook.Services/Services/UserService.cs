using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
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
        public async Task<ApiResponse<User>> AddDriverAsync(UserDTO driverDTO)
        {
            return await _userRepository.AddDriverAsync(driverDTO);
        }

        public async Task<User?> GetDriverByIdAsync(Guid id)
        {
            return await _userRepository.GetDriverByIdAsync(id);
        }

        public async Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO driverDTO)
        {
            return await _userRepository.UpdateDriverAsync(id, driverDTO);
        }

        public async Task<ApiResponse<bool>> DeleteDriverAsync(Guid id)
        {
            return await _userRepository.DeleteDriverAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllDriversAsync()
        {
            return await _userRepository.GetAllDriversAsync();
        }
    }
}
