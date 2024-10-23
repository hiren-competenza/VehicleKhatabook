using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ApiResponse<Vehicle>> AddVehicleAsync(VehicleDTO vehicleDTO);
        Task<ApiResponse<List<Vehicle>>> GetVehicleByIdAsync(Guid id);
        Task<ApiResponse<List<Vehicle>>> GetAllVehiclesAsync(Guid userId);
        Task<ApiResponse<Vehicle>> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO);
        Task<ApiResponse<bool>> DeleteVehicleAsync(Guid id);
    }
}
