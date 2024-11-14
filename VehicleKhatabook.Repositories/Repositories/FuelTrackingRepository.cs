using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
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

        public async Task<FuelTrackingDTO> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            var fuelTracking = new FuelTracking
            {
                StartVehicleMeterReading = fuelTrackingDTO.StartVehicleMeterReading,
                EndVehicleMeterReading = fuelTrackingDTO.EndVehicleMeterReading,
                StartFuelLevelInLiters = fuelTrackingDTO.StartFuelLevelInLiters,
                EndFuelLevelInLiters = fuelTrackingDTO.EndFuelLevelInLiters,
                FuelAddedInLitersJson = JsonSerializer.Serialize(fuelTrackingDTO.FuelAddedInLiters),
            };
            // Add the entity to the database
            _context.FuelTrackings.Add(fuelTracking);
            await _context.SaveChangesAsync();

            // Return the saved entity as a DTO
            return MapToDTO(fuelTracking);
        }

        public async Task<FuelTrackingDTO> GetFuelTrackingAsync()
        {
            // Retrieve the only record in the table
            var fuelTracking = await _context.FuelTrackings.FirstOrDefaultAsync();

            if (fuelTracking == null)
            {
                return null; // Or throw an exception if no record is found
            }

            // Return the entity as a DTO
            return MapToDTO(fuelTracking);
        }

        public async Task<FuelTrackingDTO> UpdateFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            var existingFuelTracking = await _context.FuelTrackings.FirstOrDefaultAsync();

            if (existingFuelTracking == null)
            {
                return null; // Or throw an exception if no record is found
            }

            // Update the entity with new values from the DTO
            existingFuelTracking.StartVehicleMeterReading = fuelTrackingDTO.StartVehicleMeterReading;
            existingFuelTracking.EndVehicleMeterReading = fuelTrackingDTO.EndVehicleMeterReading;
            existingFuelTracking.StartFuelLevelInLiters = fuelTrackingDTO.StartFuelLevelInLiters;
            existingFuelTracking.EndFuelLevelInLiters = fuelTrackingDTO.EndFuelLevelInLiters;
            existingFuelTracking.FuelAddedInLitersJson = JsonSerializer.Serialize(fuelTrackingDTO.FuelAddedInLiters);

            // Save the changes
            _context.FuelTrackings.Update(existingFuelTracking);
            await _context.SaveChangesAsync();

            // Return the updated entity as a DTO
            return MapToDTO(existingFuelTracking);
        }
        private FuelTrackingDTO MapToDTO(FuelTracking fuelTracking)
        {
            return new FuelTrackingDTO
            {
                StartVehicleMeterReading = fuelTracking.StartVehicleMeterReading,
                EndVehicleMeterReading = fuelTracking.EndVehicleMeterReading,
                StartFuelLevelInLiters = fuelTracking.StartFuelLevelInLiters,
                EndFuelLevelInLiters = fuelTracking.EndFuelLevelInLiters,
                FuelAddedInLiters = string.IsNullOrEmpty(fuelTracking.FuelAddedInLitersJson)
                    ? new List<double>()
                    : JsonSerializer.Deserialize<List<double>>(fuelTracking.FuelAddedInLitersJson)
            };
        }
    }
}
