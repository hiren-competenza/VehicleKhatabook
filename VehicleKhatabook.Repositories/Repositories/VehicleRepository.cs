using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleKhatabookDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public VehicleRepository(VehicleKhatabookDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
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
                IsActive = vehicleDTO.IsActive,
            };
            vehicle.CreatedOn = DateTime.UtcNow;
            vehicle.IsActive = true;

            _dbContext.Vehicles.Add(vehicle);
            await _dbContext.SaveChangesAsync();
            return vehicle;
        }
        public async Task<Vehicle> GetVehicleByIdAsync(Guid id)
        {
            return await _dbContext.Vehicles.FindAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _dbContext.Vehicles.ToListAsync();
        }

        public async Task<ApiResponse<Vehicle>> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO)
        {
            var response = new ApiResponse<Vehicle>();
            try { 
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserID == vehicleDTO.UserId);
            if (!userExists)
            {
                    response.Success = false;
                    response.Message = $"User with ID {vehicleDTO.UserId} does not exist.";
                    return response;
            }

            var vehicleExist = await _dbContext.Vehicles.FirstOrDefaultAsync(u => u.VehicleID == id);
            if (vehicleExist == null)
            {
                    response.Success = false;
                    response.Message = $"Vehicle with ID {id} does not exist.";
                    return response;
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

            await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Message = "Vehicle updated successfully.";
                response.Data = vehicleExist;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An unexpected error occurred. Please contact support.";
                return response;
            }
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            var vehicle = await GetVehicleByIdAsync(id);
            if (vehicle == null) return false;

            _dbContext.Vehicles.Remove(vehicle);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
