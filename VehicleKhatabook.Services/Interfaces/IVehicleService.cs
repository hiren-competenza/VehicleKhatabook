using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle> AddVehicleAsync(VehicleDTO vehicleDTO);
        Task<Vehicle> GetVehicleByIdAsync(Guid id);
        Task<(bool IsUserActive, bool HasVehicles, List<Vehicle>? Vehicles)> GetAllVehiclesAsync(Guid userId);
        //Task<ApiResponse<List<Vehicle>>> GetAllVehiclesAsync(Guid userId);
        Task<Vehicle> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO);
        Task<bool> DeleteVehicleAsync(Guid id);
    }
}
