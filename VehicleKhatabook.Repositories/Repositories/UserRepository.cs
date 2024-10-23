using BCrypt.Net;
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

        public async Task<ApiResponse<User>> AddUserAsync(UserDTO userDTO)
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
                Language = userDTO.Language,
                CreatedOn = DateTime.UtcNow,
                UserTypeId = userDTO.UserTypeId,
                Email = userDTO.Email,
                //CreatedBy = Guid.NewGuid(),
                IsActive = true
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return ApiResponse<User>.SuccessResponse(user, "User Added successfull");
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
            user.mPIN = BCrypt.Net.BCrypt.HashPassword(userDTO.mPIN);
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
        public async Task<ApiResponse<UserDetailsDTO>> AuthenticateUser(UserLoginDTO userLogin)
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
                    Email = user.Email,
                    RoleId = user.UserTypeId,
                    RoleName = user.Role
                };

                return ApiResponse<UserDetailsDTO>.SuccessResponse(userDetailsDTO, "User authenticated successfully.");
            }
            return ApiResponse<UserDetailsDTO>.FailureResponse("Invalid mobile number or mPIN.");
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

            return ApiResponse<User>.SuccessResponse(driver, "New Driver added successfully.");
            //{
            //    Success = true,
            //    Data = driver
            //};
        }

        public async Task<ApiResponse<User?>> GetDriverByIdAsync(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            return user != null ? ApiResponse<User?>.SuccessResponse(user, "User found successfully") : ApiResponse<User?>.FailureResponse("Failed to load.") ;
        }

        public async Task<ApiResponse<User>> UpdateDriverAsync(Guid id, UserDTO userDTO)
        {
            var driver = await _dbContext.Users.FindAsync(id);
            if (driver == null)
            {
                return ApiResponse<User>.FailureResponse("Driver not found.");
                //{
                //    Success = false,
                //    Message = "Driver not found."
                //};
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

            return ApiResponse<User>.SuccessResponse(driver, "driver details update successfull");
            //{
            //    Success = true,
            //    Data = driver
            //};
        }

        public async Task<ApiResponse<bool>> DeleteDriverAsync(Guid id)
        {
            var driver = await _dbContext.Users.FindAsync(id); ;
            if (driver == null)
            {
                return ApiResponse<bool>.FailureResponse("Driver not found.");
            };
            driver.IsActive = false;
            _dbContext.Users.Update(driver);
            await _dbContext.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Drver Deactive successful");
        }

        public async Task<ApiResponse<List<User>>> GetAllDriversAsync()
        {
            //var userExists = await _dbContext.Users.AnyAsync(u => u.UserID == userId && u.IsActive == true);
            //if (!userExists)
            //{
            //    return ApiResponse<List<User>>.FailureResponse($"User with ID {userId} does not exist or is inactive.");
            //}
            var drivers = await _dbContext.Users
                                  .Where(u => u.IsActive == true && u.Role.ToLower() == "driver")
                                  .ToListAsync();

            if (drivers == null || drivers.Count == 0)
            {
                return ApiResponse<List<User>>.FailureResponse($"No drivers found for user with ID.");
            }
            return ApiResponse<List<User>>.SuccessResponse(drivers, "Drivers retrieved successfully.");
        }
    }
}
