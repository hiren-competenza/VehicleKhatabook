using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IFuelTrackingRepository
    {
        /// <summary>
        /// Adds a new fuel tracking record to the database.
        /// </summary>
        /// <param name="fuelTrackingDTO">The fuel tracking details.</param>
        /// <returns>The added fuel tracking record as a DTO.</returns>
        Task<FuelTrackingDTO> AddFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO);

        /// <summary>
        /// Updates an existing fuel tracking record in the database.
        /// </summary>
        /// <param name="fuelTrackingDTO">The updated fuel tracking details.</param>
        /// <returns>The updated fuel tracking record as a DTO.</returns>
        Task<FuelTrackingDTO?> UpdateFuelTrackingAsync(FuelTrackingDTO fuelTrackingDTO);

        /// <summary>
        /// Retrieves the current fuel tracking record from the database.
        /// </summary>
        /// <returns>The fuel tracking record as a DTO, or null if no record exists.</returns>
        Task<FuelTrackingDTO?> GetFuelTrackingAsync(Guid userId);

        /// <summary>
        /// Starts a new trip by truncating existing data and adding a new record.
        /// </summary>
        /// <param name="fuelTrackingDTO">The new fuel tracking details.</param>
        /// <returns>The added fuel tracking record as a DTO.</returns>
        Task<FuelTrackingDTO> StartTripAsync(FuelTrackingDTO fuelTrackingDTO, Guid userId);

        /// <summary>
        /// Ends the current trip by calculating mileage and truncating the data.
        /// </summary>
        /// <returns>
        /// A tuple containing:
        /// - The current fuel tracking data as a DTO.
        /// - The calculated mileage as a double, or null if the calculation is invalid.
        /// </returns>
        Task<(FuelTrackingDTO?, decimal?)> EndTripAsync(Guid userId);

        Task DeleteAllFuelTrackingAsync(Guid userId);
    }

}
