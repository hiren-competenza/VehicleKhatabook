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

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> UpdateUserAsync(UserDTO userDTO)
        {
            var user = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                MobileNumber = userDTO.MobileNumber,
                mPIN = BCrypt.Net.BCrypt.HashPassword(userDTO.mPIN),
                ReferCode = userDTO.ReferCode,
                UserReferCode = userDTO.UserReferCode,
                Role = userDTO.Role,
                IsPremiumUser = userDTO.IsPremiumUser,
                State = userDTO.State,
                District = userDTO.District,
                LanguageTypeId = userDTO.languageTypeId,
                CreatedOn = DateTime.UtcNow,
                UserTypeId = userDTO.UserTypeId,
                //Email = userDTO.Email,
                //CreatedBy = Guid.NewGuid(),
                IsActive = true
            };
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task<bool> isMobileNumberAlreadyExists(string phoneNumber)
        {
            var v = await _userRepository.GetUserByMobileAsync(phoneNumber);
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
