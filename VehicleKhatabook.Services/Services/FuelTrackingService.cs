using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // Method to add a new fuel tracking record
        public async Task<FuelTrackingDTO> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            var result = await _fuelTrackingRepository.AddFuelTrackingAsync(fuelTrackingDTO);

            // Return result or null if not successful
            return result ?? null;  // returns null if the result is null, otherwise returns the FuelTrackingDTO
        }

        // Method to update an existing fuel tracking record
        public async Task<FuelTrackingDTO> UpdateFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            var result = await _fuelTrackingRepository.UpdateFuelTrackingAsync(fuelTrackingDTO);

            // Return result or null if not successful
            return result ?? null;  // returns null if the result is null, otherwise returns the updated FuelTrackingDTO
        }

        // Method to retrieve the existing fuel tracking record
        public async Task<FuelTrackingDTO> GetFuelTrackingAsync()
        {
            var result = await _fuelTrackingRepository.GetFuelTrackingAsync();

            // Return result or null if no record is found
            return result ?? null;  // returns null if the result is null, otherwise returns the FuelTrackingDTO
        }
    }


}
