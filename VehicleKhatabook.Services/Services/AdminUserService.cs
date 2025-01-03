﻿using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public AdminUserService(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        public async Task<(bool Success, AdminUserDTO AdminDetails)> AuthenticateAdminAsync(AdminLoginDTO adminLoginDTO)
        {
            var admin = await _adminUserRepository.GetAdminByMobileAndPasswordAsync(adminLoginDTO.MobileNumber, adminLoginDTO.Password);

            if (admin != null)
            {
                //string token = _adminUserRepository.GenerateToken(admin); // Assuming GenerateToken method exists

                return (true, admin);
            }

            return (false, null);
        }

        public async Task<AdminUser> RegisterAdminAsync(AdminUserDTO adminUserDTO)
        {
            var adminUser = new AdminUser
            {
                FullName = adminUserDTO.FullName,
                Username = adminUserDTO.Username,
                Email = adminUserDTO.Email,
                PasswordHash = HashPassword(adminUserDTO.Password),
                Role = adminUserDTO.Role,
                SecurityQuestion = adminUserDTO.SecurityQuestion,
                SecurityAnswerHash = HashPassword(adminUserDTO.SecurityAnswer),
                MobileNumber = adminUserDTO.MobileNumber,
                IsActive = adminUserDTO.IsActive,
                CreatedOn = DateTime.UtcNow
            };

            return await _adminUserRepository.RegisterAdminAsync(adminUser);
        }

        public async Task<AdminUser> UpdateAdminAsync(AdminUserDTO adminUserDTO)
        {
            var adminUser = new AdminUser
            {
                AdminID = adminUserDTO.AdminID,
                FullName = adminUserDTO.FullName,
                Email = adminUserDTO.Email,
                MobileNumber = adminUserDTO.MobileNumber,
                Role = adminUserDTO.Role,
                IsActive = adminUserDTO.IsActive,
                ModifiedBy = 1 // Assume an admin is making this update
            };

            return await _adminUserRepository.UpdateAdminAsync(adminUser);
        }

        public async Task<IEnumerable<AdminUser>> GetAllAdminsAsync()
        {
            return await _adminUserRepository.GetAllAdminsAsync();
        }

        public async Task<AdminUser> GetAdminByIdAsync(int adminId)
        {
            return await _adminUserRepository.GetAdminByIdAsync(adminId);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

}
