using System.ComponentModel;
using System.Data;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
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

        public async Task<User> CreateUserAsync(UserDTO userDTO)
        {
            var isExists = await isMobileNumberAlreadyExists(userDTO.MobileNumber);
            if (isExists)
            {
                return new User();
            }
            else
            {
                return await _userRepository.AddUserAsync(userDTO);
            }

        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> UpdateUserAsync(UserDTO userDTO)
        {
            return await _userRepository.UpdateUserAsync(userDTO);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task<User> AddDriverAsync(UserDTO driverDTO)
        {
            return await _userRepository.AddDriverAsync(driverDTO);
        }

        public async Task<User> GetDriverByIdAsync(Guid id)
        {
            return await _userRepository.GetDriverByIdAsync(id);
        }

        public async Task<User> UpdateDriverAsync(Guid id, UserDTO driverDTO)
        {
            return await _userRepository.UpdateDriverAsync(id, driverDTO);
        }

        public async Task<bool> DeleteDriverAsync(Guid id)
        {
            return await _userRepository.DeleteDriverAsync(id);
        }

        public async Task<List<User>> GetAllDriversAsync()
        {
            return await _userRepository.GetAllDriversAsync();
        }

        public async Task<bool> isMobileNumberAlreadyExists(string phoneNumber)
        {
            var v = await _userRepository.GetUserByMobileNumberAsync(phoneNumber);
            if (v != null)
                return true;
            else return false;
        }


        public async Task<bool> UpdateUserRoleAsync(Guid userId, string role)
        {
            return await _userRepository.UpdateUserRoleAsync(userId, role);
        }
        public async Task<bool> UpdateUserLanguageAsync(Guid userId, int languageTypeId)
        {
            return await _userRepository.UpdateUserLanguageAsync(userId, languageTypeId);
        }
    }
}
