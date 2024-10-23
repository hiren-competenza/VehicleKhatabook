using FluentValidation;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Models.Filters;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class VehicleEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var vehileRoute = app.MapGroup("/api/vehicle").WithTags("Vehicle Management").RequireAuthorization("OwnerOrDriverPolicy");
            vehileRoute.MapPost("/Add", AddVehicle).AddEndpointFilter<ValidationFilter<VehicleDTO>>();
            vehileRoute.MapGet("/{id}", GetVehicleByVehicleIdAsync);
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
           return Results.Ok(result);
        }
        internal async Task<IResult> GetVehicleByVehicleIdAsync(Guid id, IVehicleService vehicleService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("The provided vehicle ID is invalid. Please provide a valid ID.");
            }
            var vehicle = await vehicleService.GetVehicleByIdAsync(id);
            return Results.Ok(vehicle);
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
            return Results.Ok(updateVehicle);
        }

        internal async Task<IResult> DeleteVehicle(Guid id, IVehicleService vehicleService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }
            var success = await vehicleService.DeleteVehicleAsync(id);
            return Results.Ok(success);
        }

        internal async Task<IResult> GetAllVehicles(Guid userId, IVehicleService vehicleService)
        {
            var vehicles = await vehicleService.GetAllVehiclesAsync(userId);
            return Results.Ok(vehicles);
        }
    }
}
