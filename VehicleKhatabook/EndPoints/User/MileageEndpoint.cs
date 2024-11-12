using Microsoft.AspNetCore.Http;
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
            mileageGroup.MapPost("/mileage", CalculateMileage);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFuelTrackingService, FuelTrackingService>();
            services.AddScoped<IFuelTrackingRepository, FuelTrackingRepository>();
        }
        private IResult CalculateMileage(MileageInputDTO input)
        {
            if (input == null || input.FuelAddedInLiters == null || !input.FuelAddedInLiters.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid input data."));
            }

            // Calculate total fuel used
            double totalFuelUsed = input.StartFuelLevelInLiters - input.EndFuelLevelInLiters + input.FuelAddedInLiters.Sum();

            // Calculate distance covered
            double distanceCovered = input.EndVehicleMeterReading - input.StartVehicleMeterReading;

            // Check for division by zero
            if (totalFuelUsed <= 0)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel used cannot be zero or negative."));
            }

            // Calculate mileage (distance per unit fuel)
            double mileage = distanceCovered / totalFuelUsed;

            var result = new { Mileage = mileage };
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Mileage calculated successfully."));
        }

        //internal async Task<IResult> AddFuelTracking(HttpContext httpContext,MileageInputDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        //{
        //    var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Results.Unauthorized();
        //    }
        //    //fuelTrackingDTO.UserId = Guid.Parse(userId);
        //    if (fuelTrackingDTO == null)
        //        return Results.Ok(ApiResponse<object>.FailureResponse("Fuel tracking details are invalid"));

        //    var result = await fuelTrackingService.AddFuelTrackingAsync(fuelTrackingDTO);
        //    if (result == null)
        //    {
        //        return Results.Ok(ApiResponse<object>.FailureResponse("Failed to add fuel"));
        //    }
        //    return Results.Ok(ApiResponse<object>.SuccessResponse(result,"Fuel added successful."));
        //}

        //internal async Task<IResult> GetFuelTracking(Guid id, IFuelTrackingService fuelTrackingService)
        //{
        //    var result = await fuelTrackingService.GetFuelTrackingByIdAsync(id);
        //    return result == null ? Results.NotFound("Fuel log not found") : Results.Ok(result);
        //}

        //internal async Task<IResult> GetAllFuelTrackings(IFuelTrackingService fuelTrackingService)
        //{
        //    var result = await fuelTrackingService.GetAllFuelTrackingsAsync();
        //    return Results.Ok(result);
        //}
    }
}
