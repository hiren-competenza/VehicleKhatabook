using Newtonsoft.Json;
using System.Security.Claims;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class MileageEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var mileageGroup = app.MapGroup("/api/fuel").WithTags("Mileage Calculation").RequireAuthorization("OwnerOrDriverPolicy");
            mileageGroup.MapPost("/starttrip", StartTrip);
            mileageGroup.MapPut("/updatefuel", UpdateFuel);
            mileageGroup.MapGet("/get", GetFuelTracking);
            mileageGroup.MapGet("/calculatemileage", CalculateMileage);
            mileageGroup.MapPost("/endtrip", EndTrip);
        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFuelTrackingRepository, FuelTrackingRepository>();
            services.AddScoped<IFuelTrackingService, FuelTrackingService>();
        }

        // StartTrip: Add new record (handles clearing existing data and adding a new trip)
        private async Task<IResult> StartTrip(HttpContext httpContext, FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            if (fuelTrackingDTO == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid fuel tracking data."));
            }

            // Set UserId in DTO explicitly
            fuelTrackingDTO.UserId = Guid.Parse(userId);

            // Start a new trip (this handles truncating old records and adding a new one)
            var addedResult = await fuelTrackingService.StartTripAsync(fuelTrackingDTO, Guid.Parse(userId));

            return addedResult != null
                ? Results.Ok(ApiResponse<object>.SuccessResponse(addedResult, "New trip started successfully."))
                : Results.Ok(ApiResponse<object>.FailureResponse("Failed to start a new trip."));
        }

        // UpdateFuel: Update the fuel tracking record
        private async Task<IResult> UpdateFuel(HttpContext httpContext, FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            if (fuelTrackingDTO == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid fuel tracking data."));
            }

            // Set UserId in DTO explicitly
            fuelTrackingDTO.UserId = Guid.Parse(userId);

            // Update the fuel tracking record
            var updateResult = await fuelTrackingService.UpdateFuelTrackingAsync(fuelTrackingDTO);

            return updateResult != null
                ? Results.Ok(ApiResponse<object>.SuccessResponse(updateResult, "Fuel tracking updated successfully."))
                : Results.Ok(ApiResponse<object>.FailureResponse("Failed to update the fuel tracking record."));
        }

        // CalculateMileage: Calculate mileage based on fuel tracking
        private async Task<IResult> CalculateMileage(HttpContext httpContext, IFuelTrackingService fuelTrackingService)
        {
            // Retrieve the UserId from the HttpContext (if needed for validation, can be optional)
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            // Deserialize the body to get the FuelTrackingDTO object
            var fuelTrack = await httpContext.Request.ReadFromJsonAsync<FuelTrackingDTO>();
            if (fuelTrack == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid fuel tracking data."));
            }

            // Validate that the required data is present
            if (fuelTrack.StartVehicleMeterReading == 0 || fuelTrack.EndVehicleMeterReading == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Both start and end vehicle meter readings are required."));
            }

            if (fuelTrack.StartFuelLevelInLiters <= 0 || fuelTrack.EndFuelLevelInLiters == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Both start and end fuel levels are required and must be greater than zero."));
            }

            if (fuelTrack.FuelAddedInLiters == null || !fuelTrack.FuelAddedInLiters.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel added data is required."));
            }
            // Calculate total fuel used
            decimal totalFuelUsed =
                fuelTrack.StartFuelLevelInLiters - (fuelTrack.EndFuelLevelInLiters ?? 0m) // Handle null with ?? 0m (for decimal)
                + (fuelTrack.FuelAddedInLiters?.Sum() ?? 0m); // Sum() returns a double, so convert to decimal if needed.

            // Calculate distance covered
            decimal distanceCovered = (fuelTrack.EndVehicleMeterReading ?? 0) - fuelTrack.StartVehicleMeterReading;

            if (totalFuelUsed <= 0)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel used cannot be zero or negative."));
            }

            // Calculate mileage (distance per unit fuel)
            decimal mileage = distanceCovered / totalFuelUsed;

            // Return the result as JSON
            var result = new { Mileage = Math.Round(mileage, 2) };  // Round to 2 decimal places
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Mileage calculated and trip ended successfully."));
        }
        // EndTrip: End trip by calculating mileage and truncating data
        private async Task<IResult> EndTrip(HttpContext httpContext, FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        {
            // Step 1: Retrieve UserId from HttpContext
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var userGuid = Guid.Parse(userId);  // Convert UserId to Guid

            // Step 2: Ensure the DTO contains the required data (EndVehicleMeterReading, EndFuelLevelInLiters)
            if (fuelTrackingDTO.EndVehicleMeterReading == null || fuelTrackingDTO.EndFuelLevelInLiters == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("End vehicle meter reading and end fuel level are required."));
            }
            fuelTrackingDTO.UserId = userGuid;
            // Step 3: Update the database with the provided data (EndVehicleMeterReading, EndFuelLevelInLiters)
            var fuelTracking = await fuelTrackingService.UpdateFuelTrackingAsync(fuelTrackingDTO);
            if (fuelTracking == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("No fuel tracking data found for this user."));
            }

            // Step 4: Calculate mileage based on the updated data
            decimal totalFuelUsed = fuelTracking.StartFuelLevelInLiters - (fuelTracking.EndFuelLevelInLiters ?? 0) + (fuelTracking.FuelAddedInLiters?.Sum() ?? 0);
            decimal distanceCovered = (fuelTracking.EndVehicleMeterReading ?? 0) - fuelTracking.StartVehicleMeterReading;

            if (totalFuelUsed <= 0)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid fuel data, fuel used cannot be zero or negative."));
            }

            // Step 5: Calculate mileage (distance / fuel used)
            decimal mileage = distanceCovered / totalFuelUsed;

            // Step 6: Optionally, store mileage in the database (if required, depending on business logic)
            // You could save this mileage in a history table or user record if necessary.

            // Step 7: After calculating mileage, delete the fuel tracking data (or clear the current trip)
            await fuelTrackingService.DeleteAllFuelTrackingAsync(userGuid);

            // Step 8: Return the calculated mileage to the user
            var result = new { Mileage = Math.Round(mileage, 2) };
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Mileage calculated and trip ended successfully."));
        }

        private async Task<IResult> GetFuelTracking(HttpContext httpContext, IFuelTrackingService fuelTrackingService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            // Fetch the single fuel tracking record for the user
            var fuelTrackingData = await fuelTrackingService.GetFuelTrackingAsync(Guid.Parse(userId));
            if (fuelTrackingData == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("No fuel tracking data found for this user."));
            }
            // Return the result
            return Results.Ok(ApiResponse<object>.SuccessResponse(fuelTrackingData, "Fuel tracking data retrieved successfully."));
        }
    }



}
