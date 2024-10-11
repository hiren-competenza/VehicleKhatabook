using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class DriverEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var driverRoute = app.MapGroup("/api/driver").WithTags("Driver Management");

            driverRoute.MapPost("/Add", AddDriver);
            driverRoute.MapGet("/{id}", GetDriverDetails);
            driverRoute.MapPut("/{id}", UpdateDriver);
            driverRoute.MapDelete("/{id}", DeleteDriver);
            driverRoute.MapGet("/all", GetAllDrivers);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IDriverRepository, DriverRepository>();
        }

        internal async Task<IResult> AddDriver(UserDTO userDTO, IDriverService driverService)
        {
            if (userDTO == null)
                return Results.BadRequest("Driver details are invalid");

            var result = await driverService.AddDriverAsync(userDTO);
            if (result != null)
            {
                return Results.Created($"/api/driver/{result.Data.UserID}", result);
            }
            return Results.Conflict("Unable to create driver");
        }

        internal async Task<IResult> GetDriverDetails(Guid id, IDriverService driverService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }

            var driver = await driverService.GetDriverByIdAsync(id);
            return driver != null ? Results.Ok(driver) : Results.NotFound("Driver not found.");
        }

        internal async Task<IResult> UpdateDriver(Guid id, UserDTO userDTO, IDriverService driverService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }
            if (userDTO == null)
            {
                return Results.BadRequest("Invalid request body");
            }

            var updateDriver = await driverService.UpdateDriverAsync(id, userDTO);
            if (updateDriver.Success)
            {
                return Results.Ok(updateDriver.Data);
            }

            return updateDriver.Data == null
                ? Results.NotFound(updateDriver.Message)
                : Results.Conflict(updateDriver.Message);
        }

        internal async Task<IResult> DeleteDriver(Guid id, IDriverService driverService)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest("Invalid Id.");
            }

            var result = await driverService.DeleteDriverAsync(id);
            return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        }

        internal async Task<IResult> GetAllDrivers(IDriverService driverService)
        {
            var drivers = await driverService.GetAllDriversAsync();
            return Results.Ok(drivers);
        }
    }
}
