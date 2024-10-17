using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VehicleKhatabookDbContext _dbContext;

        public UserRepository(VehicleKhatabookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(UserDTO userDTO)
        {
            var userReferCode = GenerateReferCode();
            var user = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                MobileNumber = userDTO.MobileNumber,
                mPIN = userDTO.mPIN,
                ReferCode = userDTO.ReferCode,
                UserReferCode = userReferCode,
                Role = userDTO.Role,
                IsPremiumUser = userDTO.IsPremiumUser,
                State = userDTO.State,
                District = userDTO.District,
                Language = userDTO.Language,
                CreatedOn = DateTime.UtcNow,
                UserTypeId = userDTO.UserTypeId,
                Email = userDTO.Email,
                //CreatedBy = Guid.NewGuid(),
                IsActive = true
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobileNumber = user.MobileNumber,
                mPIN = user.mPIN,
                UserReferCode = user.UserReferCode,
                Role = user.Role,
                IsPremiumUser = user.IsPremiumUser,
                State = user.State,
                District = user.District,
                Language = user.Language,
                IsActive = user.IsActive
            };
        }

        public async Task UpdateUserAsync(Guid id, UserDTO userDTO)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null) return;

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.MobileNumber = userDTO.MobileNumber;
            //user.mPIN = userDTO.mPIN;
            //user.Role = userDTO.Role;
            //user.IsPremiumUser = userDTO.IsPremiumUser;
            user.State = userDTO.State;
            user.District = userDTO.District;
            user.Language = userDTO.Language;
            user.LastModifiedOn = DateTime.UtcNow;
            //user.ModifiedBy = Guid.NewGuid(); 

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null) return false;

            user.IsActive = false;
            _dbContext.Users.Update(user);
            //_dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users.Select(user => new UserDTO
            {
                UserId = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobileNumber = user.MobileNumber,
                mPIN = user.mPIN,
                ReferCode = user.ReferCode,
                Role = user.Role,
                IsPremiumUser = user.IsPremiumUser,
                State = user.State,
                District = user.District,
                Language = user.Language,
                IsActive = user.IsActive
            });
        }
        public async Task<User> GetUserByMobileNumberAsync(string mobileNumber)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
        }
        public static string GenerateReferCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
        public async Task<ApiResponse<User>> AddDriverAsync(UserDTO UserDTO)
        {
            var driver = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = UserDTO.FirstName,
                LastName = UserDTO.LastName,
                MobileNumber = UserDTO.MobileNumber,
                mPIN = UserDTO.mPIN,
                UserReferCode = UserDTO.UserReferCode,
                ReferCode = UserDTO.ReferCode,
                Role = UserDTO.Role,
                IsPremiumUser = UserDTO.IsPremiumUser,
                State = UserDTO.State,
                District = UserDTO.District,
                Language = UserDTO.Language,
                CreatedOn = DateTime.UtcNow,
                //CreatedBy = Guid.NewGuid(),
                IsActive = true
            };

            await _dbContext.Users.AddAsync(driver);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<User>
            {
                Success = true,
                Data = driver
            };
        }

        public async Task<User?> GetDriverByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO userDTO)
        {
            var driver = await GetDriverByIdAsync(id);
            if (driver == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Driver not found."
                };
            }

            driver.FirstName = userDTO.FirstName;
            driver.LastName = userDTO.LastName;
            driver.MobileNumber = userDTO.MobileNumber;
            driver.mPIN = userDTO.mPIN;
            driver.ReferCode = userDTO.ReferCode;
            driver.Role = userDTO.Role;
            driver.IsPremiumUser = userDTO.IsPremiumUser;
            driver.State = userDTO.State;
            driver.District = userDTO.District;
            driver.Language = userDTO.Language;
            driver.LastModifiedOn = DateTime.UtcNow;
            driver.IsActive = userDTO.IsActive;
            //user.ModifiedBy = Guid.NewGuid(); 

            _dbContext.Users.Update(driver);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<User>
            {
                Success = true,
                Data = driver
            };
        }

        public async Task<ApiResponse<bool>> DeleteDriverAsync(Guid id)
        {
            var driver = await GetDriverByIdAsync(id);
            if (driver == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Driver not found."
                };
            }
            driver.IsActive = false;
            _dbContext.Users.Update(driver);
            //_dbContext.Users.Remove(driver);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true
            };
        }

        public async Task<IEnumerable<User>> GetAllDriversAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}
