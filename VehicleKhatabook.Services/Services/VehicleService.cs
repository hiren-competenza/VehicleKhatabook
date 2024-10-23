using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;

        }

        public async Task<ApiResponse<Vehicle>> AddVehicleAsync(VehicleDTO vehicleDTO)
        {
            return await _vehicleRepository.AddVehicleAsync(vehicleDTO);
        }
        public async Task<ApiResponse<List<Vehicle>>> GetVehicleByIdAsync(Guid id)
        {
            return await _vehicleRepository.GetVehicleByVehicleIdAsync(id);
        }

        public async Task<ApiResponse<List<Vehicle>>> GetAllVehiclesAsync(Guid userId)
        {
            return await _vehicleRepository.GetAllVehiclesAsync(userId);
        }

        public async Task<ApiResponse<Vehicle>> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO)
        {
            return await _vehicleRepository.UpdateVehicleAsync(id,vehicleDTO);
        }

        public async Task<ApiResponse<bool>> DeleteVehicleAsync(Guid id)
        {
            var result = await _vehicleRepository.DeleteVehicleAsync(id);
            if (result)
            {
                return ApiResponse<bool>.SuccessResponse(true);
            }
            return ApiResponse<bool>.FailureResponse("Failed to Delete");
        }

    }
}
