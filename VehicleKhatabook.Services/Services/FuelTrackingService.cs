using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<FuelTrackingDTO> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            var result = await _fuelTrackingRepository.AddFuelTrackingAsync(fuelTrackingDTO);

            return result ?? null;
        }

        public async Task<FuelTrackingDTO> UpdateFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            var result = await _fuelTrackingRepository.UpdateFuelTrackingAsync(fuelTrackingDTO);

            return result ?? null;
        }

        public async Task<FuelTrackingDTO?> GetFuelTrackingAsync(Guid userId)
        {
            var result = await _fuelTrackingRepository.GetFuelTrackingAsync(userId);

            return result ?? null;
        }

        public async Task<FuelTrackingDTO> StartTripAsync(FuelTrackingDTO fuelTrackingDTO, Guid userId)
        {
            // Truncate existing records for the specific user
            await _fuelTrackingRepository.DeleteAllFuelTrackingAsync(userId);

            // Add the new fuel tracking record
            var result = await _fuelTrackingRepository.AddFuelTrackingAsync(fuelTrackingDTO);

            return result ?? null;
        }

        public async Task<(FuelTrackingDTO?, decimal?)> EndTripAsync(Guid userId)
        {
            var fuelTracking = await _fuelTrackingRepository.GetFuelTrackingAsync(userId);

            if (fuelTracking == null)
            {
                return (null, null); // No data to calculate mileage from
            }

            // Calculate mileage
            decimal totalFuelUsed = (decimal)fuelTracking.StartFuelLevelInLiters - (fuelTracking.EndFuelLevelInLiters ?? 0m) + (fuelTracking.FuelAddedInLiters?.Sum() ?? 0m);
            decimal distanceCovered = (fuelTracking.EndVehicleMeterReading ?? 0) - (decimal)fuelTracking.StartVehicleMeterReading;


            if (totalFuelUsed <= 0)
            {
                return (fuelTracking, null); // Invalid fuel data, return null mileage
            }

            decimal mileage = distanceCovered / totalFuelUsed;

            // Truncate the record after ending the trip
            await _fuelTrackingRepository.DeleteAllFuelTrackingAsync(userId);

            return (fuelTracking, mileage);
        }

        public async Task DeleteAllFuelTrackingAsync(Guid userId)
        {
            await _fuelTrackingRepository.DeleteAllFuelTrackingAsync(userId);
        }
    }
}
