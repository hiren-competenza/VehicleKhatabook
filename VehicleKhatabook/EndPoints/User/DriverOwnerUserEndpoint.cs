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
    public class DriverOwnerUserEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var driverOwnerUsers = app.MapGroup("/api/DriverOwner")
                .WithTags("DriverOwner Users")
                .RequireAuthorization("OwnerOrDriverPolicy");
            driverOwnerUsers.MapGet("/get", GetAllDriverOwnerUsers);
            driverOwnerUsers.MapGet("/getbyUser", GetDriverOwnerUserById);
            driverOwnerUsers.MapPost("/add", AddDriverOwnerUser);
            driverOwnerUsers.MapPut("/update", UpdateDriverOwnerUser);
            driverOwnerUsers.MapDelete("/delete", DeleteDriverOwnerUser);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDriverOwnerUserRepository, DriverOwnerUserRepository>();
            services.AddScoped<IDriverOwnerUserService, DriverOwnerUserService>();
        }

        // Get all DriverOwnerUsers (active only) including related User
        private async Task<IResult> GetAllDriverOwnerUsers(HttpContext context, IDriverOwnerUserService service)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found"));
            }

            var users = await service.GetAllAsync();
            if (users == null || !users.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("No DriverOwner Users found"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(users, "DriverOwner Users fetched successfully"));
        }

        // Get DriverOwnerUser by ID (including related User)
        private async Task<IResult> GetDriverOwnerUserById(HttpContext context, IDriverOwnerUserService service)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found"));
            }

            var user = await service.GetByUserAsync(Guid.Parse(userId));
            if (user == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("DriverOwner User not found"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(user, "DriverOwner User fetched successfully"));
        }

        // Add a new DriverOwnerUser
        private async Task<IResult> AddDriverOwnerUser(DriverOwnerUserDTO driverOwnerUserDTO, HttpContext context, IDriverOwnerUserService service)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found"));
            }

            // Perform the add operation
            var result = await service.AddAsync(driverOwnerUserDTO, Guid.Parse(userId));
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "DriverOwner User added successfully"));
        }

        // Update an existing DriverOwnerUser
        private async Task<IResult> UpdateDriverOwnerUser(Guid id, DriverOwnerUserDTO driverOwnerUserDTO, HttpContext context, IDriverOwnerUserService service)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found"));
            }

            // Perform the update operation
            var result = await service.UpdateAsync(id, driverOwnerUserDTO, Guid.Parse(userId));
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "DriverOwner User updated successfully"));
        }

        // Soft delete a DriverOwnerUser
        private async Task<IResult> DeleteDriverOwnerUser(Guid id, HttpContext context, IDriverOwnerUserService service)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found"));
            }

            await service.DeleteAsync(id, Guid.Parse(userId));
            return Results.Ok(ApiResponse<object>.SuccessResponse(null, "DriverOwner User deleted successfully"));
        }
    }
}
