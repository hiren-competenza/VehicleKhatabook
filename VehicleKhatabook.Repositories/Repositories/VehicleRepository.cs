﻿using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleKhatabookDbContext _dbContext;

        public VehicleRepository(VehicleKhatabookDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Vehicle> AddVehicleAsync(VehicleDTO vehicleDTO)
        {
            var vehicle = new Vehicle
            {
                UserID = vehicleDTO.UserId,
                VehicleType = vehicleDTO.VehicleType,
                RegistrationNumber = vehicleDTO.RegistrationNumber,
                NickName = vehicleDTO.NickName,
                InsuranceExpiry = vehicleDTO.InsuranceExpiry,
                PollutionExpiry = vehicleDTO.PollutionExpiry,
                FitnessExpiry = vehicleDTO.FitnessExpiry,
                RoadTaxExpiry = vehicleDTO.RoadTaxExpiry,
                RCPermitExpiry = vehicleDTO.RCPermitExpiry,
                NationalPermitExpiry = vehicleDTO.NationalPermitExpiry,
                ChassisNumber = vehicleDTO.ChassisNumber,
                EngineNumber = vehicleDTO.EngineNumber,
            };
            vehicle.CreatedOn = DateTime.UtcNow;
            vehicle.IsActive = true;

            _dbContext.Vehicles.Add(vehicle);
            await _dbContext.SaveChangesAsync();
            return vehicle;
        }
        public async Task<List<Vehicle>> GetVehicleByVehicleIdAsync(Guid id)
        {
            return await _dbContext.Vehicles.Where(i => i.VehicleID == id && i.IsActive == true).ToListAsync();
        }


        public async Task<(bool IsUserActive, bool HasVehicles, List<Vehicle>? Vehicles)> GetAllVehiclesAsync(Guid userId)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserID == userId && u.IsActive);

            if (!userExists)
            {
                return (false, false, null);
            }

            var vehicles = await _dbContext.Vehicles
                                            .Where(v => v.UserID == userId)
                                            .ToListAsync();

            if (vehicles == null || vehicles.Count == 0)
            {
                return (true, false, null);
            }

            return (true, true, vehicles);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserID == vehicleDTO.UserId && u.IsActive == true);
            if (!userExists)
            {
                    return null;
            }

            var vehicleExist = await _dbContext.Vehicles.FirstOrDefaultAsync(u => u.VehicleID == id && u.IsActive == true);
            if (vehicleExist == null)
            {
                    return null;
            }

            vehicleExist.UserID = vehicleDTO.UserId;
            vehicleExist.VehicleType = vehicleDTO.VehicleType;
            vehicleExist.RegistrationNumber = vehicleDTO.RegistrationNumber;
            vehicleExist.NickName = vehicleDTO.NickName;
            vehicleExist.InsuranceExpiry = vehicleDTO.InsuranceExpiry;
            vehicleExist.PollutionExpiry = vehicleDTO.PollutionExpiry;
            vehicleExist.FitnessExpiry = vehicleDTO.FitnessExpiry;
            vehicleExist.RoadTaxExpiry = vehicleDTO.RoadTaxExpiry;
            vehicleExist.RCPermitExpiry = vehicleDTO.RCPermitExpiry;
            vehicleExist.NationalPermitExpiry = vehicleDTO.NationalPermitExpiry;
            vehicleExist.ChassisNumber = vehicleDTO.ChassisNumber;
            vehicleExist.EngineNumber = vehicleDTO.EngineNumber;
            vehicleExist.IsActive = vehicleDTO.IsActive;

            _dbContext.Vehicles.Update(vehicleExist);
            await _dbContext.SaveChangesAsync();
            return vehicleExist;
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(id);

            if (vehicle == null || !vehicle.IsActive)
                return false;

            vehicle.IsActive = false;
            _dbContext.Vehicles.Update(vehicle);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
