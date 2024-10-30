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
    public class FuelTrackingEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var fuelRoute = app.MapGroup("/api/fuel-tracking").WithTags("Fuel Tracking")/*.RequireAuthorization("OwnerOrDriverPolicy")*/;
            fuelRoute.MapPost("/", AddFuelTracking);
            //fuelRoute.MapGet("/{id}", GetFuelTracking);
            //fuelRoute.MapPut("/{id}", UpdateFuelTracking);
            //fuelRoute.MapGet("/all", GetAllFuelTrackings);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFuelTrackingService, FuelTrackingService>();
            services.AddScoped<IFuelTrackingRepository, FuelTrackingRepository>();
        }
        internal async Task<IResult> AddFuelTracking(HttpContext httpContext,FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }
            fuelTrackingDTO.UserId = Guid.Parse(userId);
            if (fuelTrackingDTO == null)
                return Results.Ok(ApiResponse<object>.FailureResponse("Fuel tracking details are invalid"));

            var result = await fuelTrackingService.AddFuelTrackingAsync(fuelTrackingDTO);
            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to add fuel"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result,"Fuel added successful."));
        }

        internal async Task<IResult> GetFuelTracking(Guid id, IFuelTrackingService fuelTrackingService)
        {
            var result = await fuelTrackingService.GetFuelTrackingByIdAsync(id);
            return result == null ? Results.NotFound("Fuel log not found") : Results.Ok(result);
        }

        //internal async Task<IResult> UpdateFuelTracking(Guid id, FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        //{
        //    var result = await fuelTrackingService.UpdateFuelTrackingAsync(id, fuelTrackingDTO);
        //    return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
        //}

        internal async Task<IResult> GetAllFuelTrackings(IFuelTrackingService fuelTrackingService)
        {
            var result = await fuelTrackingService.GetAllFuelTrackingsAsync();
            return Results.Ok(result);
        }
    }
}
