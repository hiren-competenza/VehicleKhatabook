using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IFuelTrackingRepository
    {
        Task<ApiResponse<FuelTracking>> AddFuelTrackingAsync(FuelTracking fuelTracking);
        Task<FuelTracking?> GetFuelTrackingByIdAsync(Guid id);
        Task<ApiResponse<FuelTracking>> UpdateFuelTrackingAsync(Guid id, FuelTracking fuelTracking);
        Task<IEnumerable<FuelTracking>> GetAllFuelTrackingsAsync();
    }
}
