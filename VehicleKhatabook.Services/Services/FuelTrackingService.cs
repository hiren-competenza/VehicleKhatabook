using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class FuelTrackingService : IFuelTrackingService
    {
        private readonly IFuelTrackingRepository _fuelTrackingRepository;

        public FuelTrackingService(IFuelTrackingRepository fuelTrackingRepository)
        {
            _fuelTrackingRepository = fuelTrackingRepository;
        }

        public async Task<ApiResponse<FuelTracking>> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            var fuelTracking = new FuelTracking
            {
                VehicleID = fuelTrackingDTO.VehicleID,
                UserId = fuelTrackingDTO.UserId,
                StartMeterReading = fuelTrackingDTO.StartMeterReading,
                EndMeterReading = fuelTrackingDTO.EndMeterReading,
                StartFuelLevel = fuelTrackingDTO.StartFuelLevel,
                EndFuelLevel = fuelTrackingDTO.EndFuelLevel,
                FuelAdded = fuelTrackingDTO.FuelAdded,
                TripStartDate = fuelTrackingDTO.TripStartDate,
                TripEndDate = fuelTrackingDTO.TripEndDate
            };

            fuelTracking.Mileage = (fuelTracking.EndMeterReading - fuelTracking.StartMeterReading) /
                                   (fuelTracking.EndFuelLevel + fuelTracking.FuelAdded - fuelTracking.StartFuelLevel);

            return await _fuelTrackingRepository.AddFuelTrackingAsync(fuelTracking);
        }

        public async Task<ApiResponse<FuelTracking?>> GetFuelTrackingByIdAsync(Guid id)
        {
            return await _fuelTrackingRepository.GetFuelTrackingByIdAsync(id);
        }

        public async Task<ApiResponse<FuelTracking>> UpdateFuelTrackingAsync(Guid id, FuelTrackingDTO fuelTrackingDTO)
        {
            var updatedFuelTracking = new FuelTracking
            {
                VehicleID = fuelTrackingDTO.VehicleID,
                UserId = fuelTrackingDTO.UserId,
                StartMeterReading = fuelTrackingDTO.StartMeterReading,
                EndMeterReading = fuelTrackingDTO.EndMeterReading,
                StartFuelLevel = fuelTrackingDTO.StartFuelLevel,
                EndFuelLevel = fuelTrackingDTO.EndFuelLevel,
                FuelAdded = fuelTrackingDTO.FuelAdded,
                TripStartDate = fuelTrackingDTO.TripStartDate,
                TripEndDate = fuelTrackingDTO.TripEndDate
            };
            updatedFuelTracking.Mileage = (updatedFuelTracking.EndMeterReading - updatedFuelTracking.StartMeterReading) /
                                   (updatedFuelTracking.EndFuelLevel + updatedFuelTracking.FuelAdded - updatedFuelTracking.StartFuelLevel);
            return await _fuelTrackingRepository.UpdateFuelTrackingAsync(id, updatedFuelTracking);
        }

        public async Task<IEnumerable<FuelTracking>> GetAllFuelTrackingsAsync()
        {
            return await _fuelTrackingRepository.GetAllFuelTrackingsAsync();
        }
    }
}
