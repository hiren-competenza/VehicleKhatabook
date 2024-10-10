using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
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

        public async Task<Vehicle> AddVehicleAsync(VehicleDTO vehicleDTO)
        {
            return await _vehicleRepository.AddVehicleAsync(vehicleDTO);
        }
        public async Task<Vehicle> GetVehicleByIdAsync(Guid id)
        {
            return await _vehicleRepository.GetVehicleByIdAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllVehiclesAsync();
        }

        public async Task<ApiResponse<Vehicle>> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO)
        {
            return await _vehicleRepository.UpdateVehicleAsync(id,vehicleDTO);
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            return await _vehicleRepository.DeleteVehicleAsync(id);
        }
    }
}
