using VehicleKhatabook.Entities.Models;
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
            return await _vehicleRepository.GetVehicleByVehicleIdAsync(id);
        }

        public async Task<(bool IsUserActive, bool HasVehicles, List<Vehicle>? Vehicles)> GetAllVehiclesAsync(Guid userId)
        {
            return await _vehicleRepository.GetAllVehiclesAsync(userId);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO)
        {
            return await _vehicleRepository.UpdateVehicleAsync(id,vehicleDTO);
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            return await _vehicleRepository.DeleteVehicleAsync(id);
        }

    }
}
