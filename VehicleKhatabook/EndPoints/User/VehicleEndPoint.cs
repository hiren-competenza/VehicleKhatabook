using FluentValidation;
using System.Security.Claims;
using VehicleKhatabook.Entities.Models;
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
            vehileRoute.MapGet("/GetVehicleByVehicleId", GetVehicleByVehicleIdAsync);
            vehileRoute.MapPut("/UpdateVehicleById", UpdateVehicle).AddEndpointFilter<ValidationFilter<VehicleDTO>>();
            vehileRoute.MapDelete("/DeleteVehicleById", DeleteVehicle);
            vehileRoute.MapGet("/GetAllVehiclesByUserId", GetAllVehicles);
        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddValidatorsFromAssemblyContaining<AddVehicleValidator>();
        }

        internal async Task<IResult> AddVehicle(HttpContext httpContext, VehicleDTO vehicleDTO, IVehicleService vehicleService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            vehicleDTO.UserId = Guid.Parse(userId);
            vehicleDTO.InsuranceExpiry = string.IsNullOrWhiteSpace(vehicleDTO.InsuranceExpiry.ToString()) ? null : vehicleDTO.InsuranceExpiry;
            vehicleDTO.PollutionExpiry = string.IsNullOrWhiteSpace(vehicleDTO.PollutionExpiry.ToString()) ? null : vehicleDTO.PollutionExpiry;
            vehicleDTO.FitnessExpiry = string.IsNullOrWhiteSpace(vehicleDTO.FitnessExpiry.ToString()) ? null : vehicleDTO.FitnessExpiry;
            vehicleDTO.RoadTaxExpiry = string.IsNullOrWhiteSpace(vehicleDTO.RoadTaxExpiry.ToString()) ? null : vehicleDTO.RoadTaxExpiry;
            vehicleDTO.RCPermitExpiry = string.IsNullOrWhiteSpace(vehicleDTO.RCPermitExpiry.ToString()) ? null : vehicleDTO.RCPermitExpiry;
            vehicleDTO.NationalPermitExpiry = string.IsNullOrWhiteSpace(vehicleDTO.NationalPermitExpiry.ToString()) ? null : vehicleDTO.NationalPermitExpiry;
            vehicleDTO.ChassisNumber = string.IsNullOrWhiteSpace(vehicleDTO.ChassisNumber) ? null : vehicleDTO.ChassisNumber;
            vehicleDTO.EngineNumber = string.IsNullOrWhiteSpace(vehicleDTO.EngineNumber) ? null : vehicleDTO.EngineNumber;
            vehicleDTO.IsActive = string.IsNullOrWhiteSpace(vehicleDTO.IsActive.ToString()) ? null : vehicleDTO.IsActive;

            var result = await vehicleService.AddVehicleAsync(vehicleDTO);

            if (result != null)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(result, "Vehicle added successfully."));
            }
            return Results.Ok(ApiResponse<object>.FailureResponse("Failed to add vehicle."));
        }
        internal async Task<IResult> GetVehicleByVehicleIdAsync(HttpContext httpContext, Guid id, IVehicleService vehicleService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Request. Please login again"));
            }
            if (id == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("The vehicle ID is invalid."));
            }
            var vehicle = await vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse($"No records found for vechile ID {id}."));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(vehicle, "Vehicle Get successfull"));
        }
        internal async Task<IResult> UpdateVehicle(HttpContext httpContext, Guid id, VehicleDTO vehicleDTO, IVehicleService vehicleService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Request. Please login again"));
            }
            if (id == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Id."));
            }
            if (vehicleDTO == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid request Body"));
            }
            vehicleDTO.UserId = Guid.Parse(userId);
            var updateVehicle = await vehicleService.UpdateVehicleAsync(id, vehicleDTO);
            if (updateVehicle == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User or Vehicle does not exist."));
            }

            return Results.Ok(ApiResponse<object>.SuccessResponse(updateVehicle, "Vehicle updated successfully."));
        }
        internal async Task<IResult> DeleteVehicle(HttpContext httpContext, Guid id, IVehicleService vehicleService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Request. Please login again"));
            }
            if (id == Guid.Empty)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Invalid Id."));
            }
            var success = await vehicleService.DeleteVehicleAsync(id);
            if (!success)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to delete"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(success, $"Vechicle delete successfull."));
        }
        internal async Task<IResult> GetAllVehicles(HttpContext httpContext, IVehicleService vehicleService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }
            var (isUserActive, hasVehicles, vehicles) = await vehicleService.GetAllVehiclesAsync(Guid.Parse(userId));

            if (!isUserActive)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse($"User with ID {userId} does not exist or is inactive."));
            }

            if (!hasVehicles)
            {
                return Results.Ok(ApiResponse<object>.SuccessResponse(null, $"No vehicles found for user with ID {userId}."));
            }

            return Results.Ok(ApiResponse<List<Vehicle>>.SuccessResponse(vehicles, "Vehicles retrieved successfully."));
        }
    }
}
