using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class FuelTrackingRepository : IFuelTrackingRepository
    {
        //private readonly VehicleKhatabookDbContext _context;

        //public FuelTrackingRepository(VehicleKhatabookDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<FuelTracking> AddFuelTrackingAsync(FuelTracking fuelTracking)
        //{
        //    _context.FuelTrackings.Add(fuelTracking);
        //    await _context.SaveChangesAsync();
        //    return fuelTracking;
        //}

        //public async Task<ApiResponse<FuelTracking?>> GetFuelTrackingByIdAsync(Guid id)
        //{
        //    var result = await _context.FuelTrackings.FindAsync(id);
        //    return ApiResponse<FuelTracking?>.SuccessResponse(result);
        //}

        //public async Task<ApiResponse<FuelTracking>> UpdateFuelTrackingAsync(Guid id, FuelTracking fuelTracking)
        //{
        //    var existingLog = await _context.FuelTrackings.FindAsync(id);
        //    if (existingLog == null)
        //        return ApiResponse<FuelTracking>.FailureResponse("Fuel log not found/Not Updated");

        //    await _context.SaveChangesAsync();
        //    return ApiResponse<FuelTracking>.SuccessResponse(existingLog);
        //}

        //public async Task<IEnumerable<FuelTracking>> GetAllFuelTrackingsAsync()
        //{
        //    return await _context.FuelTrackings.ToListAsync();
        //}
    }
}
