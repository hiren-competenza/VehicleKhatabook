using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicle> AddVehicleAsync(VehicleDTO vehicleDTO);
        Task<ApiResponse<List<Vehicle>>> GetVehicleByIdAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<ApiResponse<Vehicle>> UpdateVehicleAsync(Guid id, VehicleDTO vehicleDTO);
        Task<bool> DeleteVehicleAsync(Guid id);
    }
}
