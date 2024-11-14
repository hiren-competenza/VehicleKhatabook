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
    public class MileageEndpoint : IEndpointDefinition
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
    }

}
