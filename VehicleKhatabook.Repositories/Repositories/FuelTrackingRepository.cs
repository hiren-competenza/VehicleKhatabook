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
    //public class FuelTrackingRepository : IFuelTrackingRepository
    //{
    //    private readonly VehicleKhatabookDbContext _context;

    //    public FuelTrackingRepository(VehicleKhatabookDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<FuelTrackingDTO> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
    //    {
    //        var fuelTracking = new FuelTracking
    //        {
    //            StartVehicleMeterReading = fuelTrackingDTO.StartVehicleMeterReading,
    //            EndVehicleMeterReading = fuelTrackingDTO.EndVehicleMeterReading,
    //            StartFuelLevelInLiters = fuelTrackingDTO.StartFuelLevelInLiters,
    //            EndFuelLevelInLiters = fuelTrackingDTO.EndFuelLevelInLiters,
    //            FuelAddedInLitersJson = JsonSerializer.Serialize(fuelTrackingDTO.FuelAddedInLiters),
    //        };
    //        // Add the entity to the database
    //        _context.FuelTrackings.Add(fuelTracking);
    //        await _context.SaveChangesAsync();

    //        // Return the saved entity as a DTO
    //        return MapToDTO(fuelTracking);
    //    }

    //    public async Task<FuelTrackingDTO> GetFuelTrackingAsync()
    //    {
    //        // Retrieve the only record in the table
    //        var fuelTracking = await _context.FuelTrackings.FirstOrDefaultAsync();

    //        if (fuelTracking == null)
    //        {
    //            return null; // Or throw an exception if no record is found
    //        }

    //        // Return the entity as a DTO
    //        return MapToDTO(fuelTracking);
    //    }

    //    public async Task<FuelTrackingDTO> UpdateFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
    //    {
    //        var existingFuelTracking = await _context.FuelTrackings.FirstOrDefaultAsync();

    //        if (existingFuelTracking == null)
    //        {
    //            return null; // Or throw an exception if no record is found
    //        }

    //        // Update the entity with new values from the DTO
    //        existingFuelTracking.StartVehicleMeterReading = fuelTrackingDTO.StartVehicleMeterReading;
    //        existingFuelTracking.EndVehicleMeterReading = fuelTrackingDTO.EndVehicleMeterReading;
    //        existingFuelTracking.StartFuelLevelInLiters = fuelTrackingDTO.StartFuelLevelInLiters;
    //        existingFuelTracking.EndFuelLevelInLiters = fuelTrackingDTO.EndFuelLevelInLiters;
    //        existingFuelTracking.FuelAddedInLitersJson = JsonSerializer.Serialize(fuelTrackingDTO.FuelAddedInLiters);

    //        // Save the changes
    //        _context.FuelTrackings.Update(existingFuelTracking);
    //        await _context.SaveChangesAsync();

    //        // Return the updated entity as a DTO
    //        return MapToDTO(existingFuelTracking);
    //    }
    //    private FuelTrackingDTO MapToDTO(FuelTracking fuelTracking)
    //    {
    //        return new FuelTrackingDTO
    //        {
    //            StartVehicleMeterReading = fuelTracking.StartVehicleMeterReading,
    //            EndVehicleMeterReading = fuelTracking.EndVehicleMeterReading,
    //            StartFuelLevelInLiters = fuelTracking.StartFuelLevelInLiters,
    //            EndFuelLevelInLiters = fuelTracking.EndFuelLevelInLiters,
    //            FuelAddedInLiters = string.IsNullOrEmpty(fuelTracking.FuelAddedInLitersJson)
    //                ? new List<double>()
    //                : JsonSerializer.Deserialize<List<double>>(fuelTracking.FuelAddedInLitersJson)
    //        };
    //    }
    //}
    public class FuelTrackingRepository : IFuelTrackingRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public FuelTrackingRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        // StartTrip: Truncate and add new record
        public async Task<FuelTrackingDTO> StartTripAsync(FuelTrackingDTO fuelTrackingDTO, Guid userId)
        {
            // Remove the existing fuel tracking records for the specified user
            var existingRecords = await _context.FuelTrackings
                                                 .Where(f => f.UserId == userId)
                                                 .ToListAsync();

            _context.FuelTrackings.RemoveRange(existingRecords);
            await _context.SaveChangesAsync();

            // Add the new fuel tracking record with the specified userId
            fuelTrackingDTO.UserId = userId; // Ensure the UserId is included in the DTO before saving
            return await AddFuelTrackingAsync(fuelTrackingDTO);
        }


        // Add a new fuel tracking record
        public async Task<FuelTrackingDTO> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            // Get the UserId from the DTO (assuming it's passed in the DTO)
            if (fuelTrackingDTO.UserId == Guid.Empty)
            {
                throw new ArgumentException("UserId must be provided", nameof(fuelTrackingDTO.UserId));
            }

            var fuelTracking = new FuelTracking
            {
                UserId = fuelTrackingDTO.UserId,  // Set UserId to associate the record with a user
                StartVehicleMeterReading = fuelTrackingDTO.StartVehicleMeterReading,
                EndVehicleMeterReading = fuelTrackingDTO.EndVehicleMeterReading,
                StartFuelLevelInLiters = fuelTrackingDTO.StartFuelLevelInLiters,
                EndFuelLevelInLiters = fuelTrackingDTO.EndFuelLevelInLiters,
                FuelAddedInLitersJson = JsonSerializer.Serialize(fuelTrackingDTO.FuelAddedInLiters),
            };

            _context.FuelTrackings.Add(fuelTracking);
            await _context.SaveChangesAsync();

            return MapToDTO(fuelTracking);  // Map the FuelTracking entity to DTO
        }

        // Retrieve the only record in the table
        public async Task<FuelTrackingDTO?> GetFuelTrackingAsync(Guid userId)
        {
            var fuelTracking = await _context.FuelTrackings
                                              .Where(f => f.UserId == userId)
                                              .OrderByDescending(f => f.Id) // Get the latest record (assuming 'Id' is the primary key and increments)
                                              .FirstOrDefaultAsync();

            return fuelTracking == null ? null : MapToDTO(fuelTracking);
        }


        // Update the existing fuel tracking record
        public async Task<FuelTrackingDTO?> UpdateFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO)
        {
            // Ensure UserId is provided in the DTO
            if (fuelTrackingDTO.UserId == Guid.Empty)
            {
                throw new ArgumentException("UserId must be provided", nameof(fuelTrackingDTO.UserId));
            }

            // Fetch the existing FuelTracking record for the given UserId
            var existingFuelTracking = await _context.FuelTrackings
                .FirstOrDefaultAsync(f => f.UserId == fuelTrackingDTO.UserId);

            if (existingFuelTracking == null)
            {
                return null; // No record to update
            }

            // Update the existing fuel tracking record with new data from DTO
            existingFuelTracking.StartVehicleMeterReading = fuelTrackingDTO.StartVehicleMeterReading;
            existingFuelTracking.EndVehicleMeterReading = fuelTrackingDTO.EndVehicleMeterReading;
            existingFuelTracking.StartFuelLevelInLiters = fuelTrackingDTO.StartFuelLevelInLiters;
            existingFuelTracking.EndFuelLevelInLiters = fuelTrackingDTO.EndFuelLevelInLiters;
            existingFuelTracking.FuelAddedInLitersJson = JsonSerializer.Serialize(fuelTrackingDTO.FuelAddedInLiters);

            _context.FuelTrackings.Update(existingFuelTracking);
            await _context.SaveChangesAsync();

            return MapToDTO(existingFuelTracking);  // Return the updated DTO
        }


        // EndTrip: Return mileage and truncate data
        public async Task<(FuelTrackingDTO?, decimal?)> EndTripAsync(Guid userId)
        {
            // Fetch the fuel tracking record for the given UserId
            var fuelTracking = await _context.FuelTrackings
                                             .FirstOrDefaultAsync(f => f.UserId == userId);

            if (fuelTracking == null)
            {
                return (null, null); // No data found for the specified user
            }

            // Calculate the total fuel used
            decimal totalFuelUsed =
     Convert.ToDecimal(fuelTracking.StartFuelLevelInLiters)
     - Convert.ToDecimal(fuelTracking.EndFuelLevelInLiters ?? 0)  // Handle null value with ?? 0
     + (JsonSerializer.Deserialize<List<decimal>>(fuelTracking.FuelAddedInLitersJson)?.Sum() ?? 0);


            // Calculate the distance covered
            decimal distanceCovered = (fuelTracking.EndVehicleMeterReading ?? 0) - fuelTracking.StartVehicleMeterReading;


            // If total fuel used is invalid, return the data with null mileage
            if (totalFuelUsed <= 0)
            {
                return (MapToDTO(fuelTracking), null); // Invalid data for mileage calculation
            }

            // Calculate the mileage (distance per unit fuel)
            decimal mileage = distanceCovered / totalFuelUsed;

            // After mileage calculation, remove the user's fuel tracking record
            _context.FuelTrackings.Remove(fuelTracking);
            await _context.SaveChangesAsync();

            // Return the fuel tracking DTO and the calculated mileage
            return (MapToDTO(fuelTracking), mileage);
        }


        public async Task DeleteAllFuelTrackingAsync(Guid userId)
        {
            var fuelTrackings = await _context.FuelTrackings
                                               .Where(f => f.UserId == userId)
                                               .ToListAsync();

            if (fuelTrackings.Any())
            {
                _context.FuelTrackings.RemoveRange(fuelTrackings);
                await _context.SaveChangesAsync();
            }
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
                    ? new List<decimal>()
                    : JsonSerializer.Deserialize<List<decimal>>(fuelTracking.FuelAddedInLitersJson),
                // Include UserId if needed
                UserId = fuelTracking.UserId
            };
        }
    }

}
