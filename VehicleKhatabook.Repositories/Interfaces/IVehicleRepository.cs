using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> AddVehicleAsync(VehicleDTO vehicleDTO);
        Task<List<Vehicle>> GetVehicleByVehicleIdAsync(Guid id);
        //Task<ApiResponse<List<Vehicle>>> GetAllVehiclesAsync(Guid userId);
        Task<(bool IsUserActive, bool HasVehicles, List<Vehicle>? Vehicles)> GetAllVehiclesAsync(Guid userId);

        Task<Vehicle> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO);
        Task<bool> DeleteVehicleAsync(Guid id);
    }
}
