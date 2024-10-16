﻿using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IFuelTrackingService
    {
        Task<ApiResponse<FuelTracking>> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO);
        Task<FuelTracking?> GetFuelTrackingByIdAsync(Guid id);
        Task<ApiResponse<FuelTracking>> UpdateFuelTrackingAsync(Guid id, FuelTrackingDTO fuelTrackingDTO);
        Task<IEnumerable<FuelTracking>> GetAllFuelTrackingsAsync();
    }
}
