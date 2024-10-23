using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<ApiResponse<Vehicle>> AddVehicleAsync(VehicleDTO vehicleDTO);
        Task<ApiResponse<List<Vehicle>>> GetVehicleByVehicleIdAsync(Guid id);
        Task<ApiResponse<List<Vehicle>>> GetAllVehiclesAsync(Guid userId);
        Task<ApiResponse<Vehicle>> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO);
        Task<bool> DeleteVehicleAsync(Guid id);
    }
}
