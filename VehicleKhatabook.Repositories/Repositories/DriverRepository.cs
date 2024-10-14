using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly VehicleKhatabookDbContext _dbContext;

        public DriverRepository(VehicleKhatabookDbContext dbContext)
        {
            _dbContext = dbContext;
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

            _dbContext.Users.Remove(driver);
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
