using Microsoft.AspNetCore.Http;
using System.Net.Http;
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
    public class MileageEndpointOld : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var mileageGroup = app.MapGroup("/api/fuel").WithTags("Mileage Calculation").RequireAuthorization("OwnerOrDriverPolicy");
            // Endpoint for adding a new fuel tracking record
            mileageGroup.MapPost("/add", AddFuelTracking);
            // Endpoint for updating an existing fuel tracking record
            mileageGroup.MapPut("/update", UpdateFuelTracking);
            // Endpoint for retrieving the existing fuel tracking record
            mileageGroup.MapGet("/retrieve", GetFuelTracking);
            // Endpoint for calculating mileage
            mileageGroup.MapPost("/mileage", CalculateMileage);
            mileageGroup.MapPost("/starttrip", StartTrip);  // Start trip endpoint
            mileageGroup.MapPost("/endtrip", EndTrip);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register FuelTrackingService
            // Register the repository and service for FuelTracking
            services.AddScoped<IFuelTrackingRepository, FuelTrackingRepository>();  // Register the repository
            services.AddScoped<IFuelTrackingService, FuelTrackingService>();  // Register the service
        }

        // Add FuelTracking
        private async Task<IResult> AddFuelTracking(HttpContext httpContext, FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
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

            var result = await fuelTrackingService.AddFuelTrackingAsync(fuelTrackingDTO);

            return result != null
                ? Results.Ok(ApiResponse<object>.SuccessResponse(result, "Fuel tracking added successfully"))
                : Results.Ok(ApiResponse<object>.FailureResponse("Failed to add fuel tracking."));
        }

        // Update FuelTracking
        private async Task<IResult> UpdateFuelTracking(HttpContext httpContext, FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
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

            var result = await fuelTrackingService.UpdateFuelTrackingAsync(fuelTrackingDTO);

            return result != null
                ? Results.Ok(ApiResponse<object>.SuccessResponse(result, "Fuel tracking updated successfully"))
                : Results.Ok(ApiResponse<object>.FailureResponse("Failed to update fuel tracking. It may not exist."));
        }

        // Retrieve FuelTracking
        private async Task<IResult> GetFuelTracking(HttpContext httpContext, IFuelTrackingService fuelTrackingService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var fuelTracking = await fuelTrackingService.GetFuelTrackingAsync();

            if (fuelTracking == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel tracking record not found."));
            }

            return Results.Ok(ApiResponse<object>.SuccessResponse(fuelTracking, "Fuel tracking retrieved successfully"));
        }

        // Calculate Mileage based on FuelTracking
        private async Task<IResult> CalculateMileage(HttpContext httpContext, IFuelTrackingService fuelTrackingService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var fuelTracking = await fuelTrackingService.GetFuelTrackingAsync();

            if (fuelTracking == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel tracking data not found."));
            }

            // Calculate total fuel used
            double totalFuelUsed = fuelTracking.StartFuelLevelInLiters - fuelTracking.EndFuelLevelInLiters + fuelTracking.FuelAddedInLiters.Sum();
            // Calculate distance covered
            double distanceCovered = fuelTracking.EndVehicleMeterReading - fuelTracking.StartVehicleMeterReading;
            // Check for division by zero
            if (totalFuelUsed <= 0)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel used cannot be zero or negative."));
            }
            // Calculate mileage (distance per unit fuel)
            double mileage = distanceCovered / totalFuelUsed;
            var result = new { Mileage = mileage };
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Mileage calculated successfully"));
        }

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

            // Check if a record already exists for this user
            var existingRecord = await fuelTrackingService.GetFuelTrackingByUserIdAsync(Guid.Parse(userId));
            if (existingRecord != null)
            {
                // Update existing record
                fuelTrackingDTO.Id = existingRecord.Id;
                var updatedResult = await fuelTrackingService.UpdateFuelTrackingAsync(fuelTrackingDTO);
                return updatedResult != null
                    ? Results.Ok(ApiResponse<object>.SuccessResponse(updatedResult, "Fuel tracking updated successfully for the trip."))
                    : Results.Ok(ApiResponse<object>.FailureResponse("Failed to update the fuel tracking record."));
            }
            else
            {
                // Add new record
                var addedResult = await fuelTrackingService.AddFuelTrackingAsync(fuelTrackingDTO);
                return addedResult != null
                    ? Results.Ok(ApiResponse<object>.SuccessResponse(addedResult, "Fuel tracking added successfully for the trip."))
                    : Results.Ok(ApiResponse<object>.FailureResponse("Failed to add the fuel tracking record."));
            }
        }

        // EndTrip: Calculate mileage and delete the trip record
        private async Task<IResult> EndTrip(HttpContext httpContext, IFuelTrackingService fuelTrackingService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            // Fetch the existing fuel tracking record for the user
            var fuelTracking = await fuelTrackingService.GetFuelTrackingByUserIdAsync(Guid.Parse(userId));
            if (fuelTracking == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("No fuel tracking data found for the trip."));
            }

            // Calculate total fuel used
            double totalFuelUsed = fuelTracking.StartFuelLevelInLiters - fuelTracking.EndFuelLevelInLiters + fuelTracking.FuelAddedInLiters.Sum();
            // Calculate distance covered
            double distanceCovered = fuelTracking.EndVehicleMeterReading - fuelTracking.StartVehicleMeterReading;
            // Validate division
            if (totalFuelUsed <= 0)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel used cannot be zero or negative."));
            }
            // Calculate mileage (distance per unit fuel)
            double mileage = distanceCovered / totalFuelUsed;

            // Delete the fuel tracking record
            var deleteResult = await fuelTrackingService.DeleteFuelTrackingAsync(fuelTracking.Id);

            if (!deleteResult)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to delete the fuel tracking record after mileage calculation."));
            }

            var result = new { Mileage = mileage };
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Mileage calculated and trip ended successfully."));
        }
    }

}
