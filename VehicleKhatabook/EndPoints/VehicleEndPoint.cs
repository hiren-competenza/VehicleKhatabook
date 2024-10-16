using FluentValidation;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Models.Filters;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class VehicleEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var vehileRoute = app.MapGroup("/api/vehicle").WithTags("Vehicle Management");
            vehileRoute.MapPost("/Add", AddVehicle).AddEndpointFilter<ValidationFilter<VehicleDTO>>();
            vehileRoute.MapGet("/{id}", GetVehicleDetails);
            vehileRoute.MapPut("/{id}", UpdateVehicle).AddEndpointFilter<ValidationFilter<VehicleDTO>>();
            vehileRoute.MapDelete("/{id}", DeleteVehicle);
            vehileRoute.MapGet("/all", GetAllVehicles);
        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddValidatorsFromAssemblyContaining<AddVehicleValidator>();
        }

        internal async Task<IResult> AddVehicle(VehicleDTO vehicleDTO, IVehicleService vehicleService)
        {
            if (vehicleDTO == null)
                return Results.BadRequest("Vehicle details are invalid");

            var result = await vehicleService.AddVehicleAsync(vehicleDTO);
            if (result != null)
            {
                return Results.Created($"/api/vehicle/{result.VehicleID}", result);
            }
            return Results.Conflict("Unable to create Vechile");
        }
        internal async Task<IResult> GetVehicleDetails(Guid id, IVehicleService vehicleService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("The provided vehicle ID is invalid. Please provide a valid ID.");
            }
            var vehicle = await vehicleService.GetVehicleByIdAsync(id);
            return vehicle != null ? Results.Ok(vehicle) : Results.NotFound("No vehicle found with the provided ID.");
        }

        internal async Task<IResult> UpdateVehicle(Guid id, VehicleDTO vehicleDTO, IVehicleService vehicleService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }
            if (vehicleDTO == null)
            {
                return Results.BadRequest("Invalid request Body");
            }
            var updateVehicle = await vehicleService.UpdateVehicleAsync(id, vehicleDTO);

            if (updateVehicle.Success)
            {
                return Results.Ok(updateVehicle.Data);
            }
            return updateVehicle.Data == null
            ? Results.NotFound(updateVehicle.Message)
            : Results.Conflict(updateVehicle.Message);
        }

        internal async Task<IResult> DeleteVehicle(Guid id, IVehicleService vehicleService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }
                var success = await vehicleService.DeleteVehicleAsync(id);
            if (success)
            {
                return Results.Ok("Vechie deleted successfully.");
            }
            return Results.NotFound();
        }

        internal async Task<IResult> GetAllVehicles(IVehicleService vehicleService)
        {
            var vehicles = await vehicleService.GetAllVehiclesAsync();
            return Results.Ok(vehicles);
        }

    }
}
