using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class FuelTrackingRepository : IFuelTrackingRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public FuelTrackingRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<FuelTracking>> AddFuelTrackingAsync(FuelTracking fuelTracking)
        {
            _context.FuelTrackings.Add(fuelTracking);
            await _context.SaveChangesAsync();
            return new ApiResponse<FuelTracking> { Success = true, Data = fuelTracking };
        }

        public async Task<FuelTracking?> GetFuelTrackingByIdAsync(Guid id)
        {
            return await _context.FuelTrackings.FindAsync(id);
        }

        public async Task<ApiResponse<FuelTracking>> UpdateFuelTrackingAsync(Guid id, FuelTracking fuelTracking)
        {
            var existingLog = await _context.FuelTrackings.FindAsync(id);
            if (existingLog == null)
                return new ApiResponse<FuelTracking> { Success = false, Message = "Fuel log not found" };

            // Update properties here and recalculate mileage

            await _context.SaveChangesAsync();
            return new ApiResponse<FuelTracking> { Success = true, Data = existingLog };
        }

        public async Task<IEnumerable<FuelTracking>> GetAllFuelTrackingsAsync()
        {
            return await _context.FuelTrackings.ToListAsync();
        }
    }
}
