using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Models.DTOs;


namespace VehicleKhatabook.Repositories.Repositories
{
    public class DriverOwnerUserRepository : IDriverOwnerUserRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public DriverOwnerUserRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        // Get a DriverOwnerUser by ID (including the related User)
        public async Task<IEnumerable<DriverOwnerUser>> GetAllAsync()
        {
            return await _context.DriverOwnerUsers
                .Include(du => du.user) // Include related User object
                .Where(du => du.IsActive == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<DriverOwnerUser>> GetByUserAsync(Guid userId)
        {
            return await _context.DriverOwnerUsers
                .Include(du => du.user) // Include related User object
                .Where(du => du.UserID == userId && du.IsActive == true)
                .ToListAsync();
        }

        public async Task<DriverOwnerUser> AddAsync(DriverOwnerUserDTO driverOwnerUserDTO, Guid userId)
        {
            var entity = new DriverOwnerUser
            {
                DriverOwnerUserId = Guid.NewGuid(),
                UserID = userId,
                FirstName = driverOwnerUserDTO.FirstName,
                MobileNumber = driverOwnerUserDTO.MobileNumber,
                UserType = driverOwnerUserDTO.UserType,
                IsActive = true
            };

            await _context.DriverOwnerUsers.AddAsync(entity);
            await _context.SaveChangesAsync();
            await _context.Entry(entity).Reference(d => d.user).LoadAsync();
            return entity; // Return the added entity
        }

        public async Task<DriverOwnerUser> UpdateAsync(Guid id, DriverOwnerUserDTO driverOwnerUserDTO, Guid userId)
        {
            var entity = await _context.DriverOwnerUsers
                .FirstOrDefaultAsync(du => du.DriverOwnerUserId == id && du.UserID == userId && du.IsActive == true);

            if (entity == null) return null;

            entity.FirstName = driverOwnerUserDTO.FirstName;
            entity.MobileNumber = driverOwnerUserDTO.MobileNumber;
            entity.UserType = driverOwnerUserDTO.UserType;

            await _context.SaveChangesAsync();
            await _context.Entry(entity).Reference(d => d.user).LoadAsync();
            return entity; // Return the updated entity
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var entity = await _context.DriverOwnerUsers
                .FirstOrDefaultAsync(du => du.DriverOwnerUserId == id && du.UserID == userId);

            if (entity == null) return;

            entity.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
        }
    }


}
