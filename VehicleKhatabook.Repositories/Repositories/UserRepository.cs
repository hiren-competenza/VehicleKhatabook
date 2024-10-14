using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
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
            var user = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                MobileNumber = userDTO.MobileNumber,
                mPIN = userDTO.mPIN,
                ReferCode = userDTO.ReferCode,
                Role = userDTO.Role,
                IsPremiumUser = userDTO.IsPremiumUser,
                State = userDTO.State,
                District = userDTO.District,
                Language = userDTO.Language,
                CreatedOn = DateTime.UtcNow,
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
                ReferCode = user.ReferCode,
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
            user.mPIN = userDTO.mPIN;
            user.ReferCode = userDTO.ReferCode;
            user.Role = userDTO.Role;
            user.IsPremiumUser = userDTO.IsPremiumUser;
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

            _dbContext.Users.Remove(user);
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
    }
}
