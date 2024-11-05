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

        public async Task<User> AddUserAsync(UserDTO userDTO)
        {
            var userReferCode = GenerateReferCode();
            var user = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                MobileNumber = userDTO.MobileNumber,
                mPIN = BCrypt.Net.BCrypt.HashPassword(userDTO.mPIN),
                ReferCode = userDTO.ReferCode,
                UserReferCode = userReferCode,
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

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == id && u.IsActive == true);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobileNumber = user.MobileNumber,
                UserTypeId = user.UserTypeId,
                //mPIN = user.mPIN,
                UserReferCode = user.UserReferCode,
                Role = user.Role,
                IsPremiumUser = user.IsPremiumUser,
                State = user.State,
                District = user.District,
                languageTypeId = user.LanguageTypeId,
                IsActive = user.IsActive
            };
        }

        public async Task<User> UpdateUserAsync(UserDTO userDTO)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserID == userDTO.UserId && u.IsActive);
            if (user == null)
                return null;

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.MobileNumber = userDTO.MobileNumber;
            //user.mPIN = BCrypt.Net.BCrypt.HashPassword(userDTO.mPIN);
            user.UserTypeId = userDTO.UserTypeId;
            user.IsPremiumUser = userDTO.IsPremiumUser;
            user.IsActive = userDTO.IsActive;
            user.Role = userDTO.Role;
            user.State = userDTO.State;
            user.District = userDTO.District;
            user.LanguageTypeId = userDTO.languageTypeId;
            user.LastModifiedOn = DateTime.UtcNow;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
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
                languageTypeId = user.LanguageTypeId,
                IsActive = user.IsActive
            });
        }
        public async Task<UserDetailsDTO> AuthenticateUser(UserLoginDTO userLogin)
        {
            var user = await _dbContext.Users
              .Where(u => u.MobileNumber == userLogin.MobileNumber && u.IsActive).FirstOrDefaultAsync();

            if (user != null && BCrypt.Net.BCrypt.Verify(userLogin.mPIN, user.mPIN))
            {
                var userDetailsDTO = new UserDetailsDTO
                {
                    UserId = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MobileNumber = user.MobileNumber,
                    Mpin = user.mPIN,
                    //Email = user.Email,
                    RoleId = user.UserTypeId,
                    RoleName = user.Role,
                    IsPremiumUser= user.IsPremiumUser,
                    State = user.State,
                    District = user.District,
                    IsActive= user.IsActive,
                    UserReferCode = user.UserReferCode,
                    LanguageTypeId  = user.LanguageTypeId
                };
                return userDetailsDTO;
            }
            return null;
        }

        public async Task<User> GetUserByMobileNumberAsync(string mobileNumber)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
        }
        public static string GenerateReferCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
        public async Task<User> AddDriverAsync(UserDTO UserDTO)
        {
            var driver = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = UserDTO.FirstName,
                LastName = UserDTO.LastName,
                MobileNumber = UserDTO.MobileNumber,
                mPIN = BCrypt.Net.BCrypt.HashPassword(UserDTO.mPIN),
                UserReferCode = UserDTO.UserReferCode,
                ReferCode = UserDTO.ReferCode,
                Role = UserDTO.Role,
                IsPremiumUser = UserDTO.IsPremiumUser,
                State = UserDTO.State,
                District = UserDTO.District,
                LanguageTypeId = UserDTO.languageTypeId,
                CreatedOn = DateTime.UtcNow,
                //CreatedBy = Guid.NewGuid(),
                IsActive = true
            };

            await _dbContext.Users.AddAsync(driver);
            await _dbContext.SaveChangesAsync();
            return driver;
        }

        public async Task<User> GetDriverByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> UpdateDriverAsync(Guid id, UserDTO userDTO)
        {
            var driver = await _dbContext.Users.FindAsync(id);
            if (driver == null)
            {
                return null;
            }

            driver.FirstName = userDTO.FirstName;
            driver.LastName = userDTO.LastName;
            driver.MobileNumber = userDTO.MobileNumber;
            //driver.mPIN = userDTO.mPIN;
            driver.ReferCode = userDTO.ReferCode;
            driver.Role = userDTO.Role;
            driver.IsPremiumUser = userDTO.IsPremiumUser;
            driver.State = userDTO.State;
            driver.District = userDTO.District;
            driver.LanguageTypeId = userDTO.languageTypeId;
            driver.LastModifiedOn = DateTime.UtcNow;
            driver.IsActive = userDTO.IsActive;
            //user.ModifiedBy = Guid.NewGuid(); 

            _dbContext.Users.Update(driver);
            await _dbContext.SaveChangesAsync();

            return driver;
        }

        public async Task<bool> DeleteDriverAsync(Guid id)
        {
            var driver = await _dbContext.Users.FindAsync(id); ;
            if (driver == null)
            {
                return false;
            };
            driver.IsActive = false;
            _dbContext.Users.Update(driver);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<User>> GetAllDriversAsync()
        {
            var drivers = await _dbContext.Users
                                  .Where(u => u.IsActive == true && u.Role.ToLower() == "driver")
                                  .ToListAsync();
            return drivers;
        }
    }
}
