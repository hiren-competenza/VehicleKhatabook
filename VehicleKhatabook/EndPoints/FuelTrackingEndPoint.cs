using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class FuelTrackingEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var fuelRoute = app.MapGroup("/api/fuel-tracking").WithTags("Fuel Tracking");
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
        internal async Task<IResult> AddFuelTracking(FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        {
            if (fuelTrackingDTO == null)
                return Results.BadRequest("Fuel tracking details are invalid");

            var result = await fuelTrackingService.AddFuelTrackingAsync(fuelTrackingDTO);
            return result.Success ? Results.Created($"/api/fuel-tracking/{result.Data.FuelTrackingID}", result.Data)
                                  : Results.Conflict(result.Message);
        }

        internal async Task<IResult> GetFuelTracking(Guid id, IFuelTrackingService fuelTrackingService)
        {
            var result = await fuelTrackingService.GetFuelTrackingByIdAsync(id);
            return result == null ? Results.NotFound("Fuel log not found") : Results.Ok(result);
        }

        internal async Task<IResult> UpdateFuelTracking(Guid id, FuelTrackingDTO fuelTrackingDTO, IFuelTrackingService fuelTrackingService)
        {
            var result = await fuelTrackingService.UpdateFuelTrackingAsync(id, fuelTrackingDTO);
            return result.Success ? Results.Ok(result.Data) : Results.Conflict(result.Message);
        }

        internal async Task<IResult> GetAllFuelTrackings(IFuelTrackingService fuelTrackingService)
        {
            var result = await fuelTrackingService.GetAllFuelTrackingsAsync();
            return Results.Ok(result);
        }
    }
}
