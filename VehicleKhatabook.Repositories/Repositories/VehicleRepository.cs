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
        public async Task<ApiResponse<Vehicle>> AddVehicleAsync(VehicleDTO vehicleDTO)
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
            return ApiResponse<Vehicle>.SuccessResponse(vehicle, "Vehicle added successful");
        }
        public async Task<ApiResponse<List<Vehicle>>> GetVehicleByVehicleIdAsync(Guid id)
        {
            var result = await _dbContext.Vehicles.Where(i => i.VehicleID == id && i.IsActive == true).ToListAsync();

            if (result == null || result.Count == 0)
            {
                return ApiResponse<List<Vehicle>>.FailureResponse($"No records found for vechile ID {id}.");
            }

            return ApiResponse<List<Vehicle>>.SuccessResponse(result, "Vehicle List Get successfull");
        }


        public async Task<ApiResponse<List<Vehicle>>> GetAllVehiclesAsync(Guid userId)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserID == userId && u.IsActive == true);

            if (!userExists)
            {
                return ApiResponse<List<Vehicle>>.FailureResponse($"User with ID {userId} does not exist or is inactive.");
            }

            var vehicles = await _dbContext.Vehicles
                                           .Where(v => v.UserID == userId)
                                           .ToListAsync();

            if (vehicles == null || vehicles.Count == 0)
            {
                return ApiResponse<List<Vehicle>>.FailureResponse($"No vehicles found for user with ID {userId}.");
            }

            return ApiResponse<List<Vehicle>>.SuccessResponse(vehicles, "Vehicles retrieved successfully.");
        }

        public async Task<ApiResponse<Vehicle>> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.UserID == vehicleDTO.UserId && u.IsActive == true);
            if (!userExists)
            {
                    return ApiResponse<Vehicle>.FailureResponse($"User with ID {vehicleDTO.UserId} does not exist.");
            }

            var vehicleExist = await _dbContext.Vehicles.FirstOrDefaultAsync(u => u.VehicleID == id && u.IsActive == true);
            if (vehicleExist == null)
            {
                    return ApiResponse<Vehicle>.FailureResponse($"Vehicle with ID {id} does not exist.");
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
            return ApiResponse<Vehicle>.SuccessResponse(vehicleExist, "Vehicle updated successfully.");
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
