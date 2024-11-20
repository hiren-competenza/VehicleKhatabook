using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class VehicleTypesEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/master").WithTags("Vehicle Types Management")/*.RequireAuthorization("AdminPolicy")*/;
            staticRoute.MapPost("/addVehicleType", AddVehicleTypesAsync);
            staticRoute.MapPost("/updateVehicleType", UpdateVehicleTypeAsync);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        internal async Task<IResult> AddVehicleTypesAsync(VechileTypeDTO vechileType, IMasterDataService masterDataService)
        {
            var vehicleTypes = await masterDataService.AddVehicleTypesAsync(vechileType);
            return Results.Ok(vehicleTypes);
        }
        internal async Task<IResult> UpdateVehicleTypeAsync(int vehicleTypeId, VechileTypeDTO vehicleTypeDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateVehicleTypeAsync(vehicleTypeId, vehicleTypeDTO);
            return Results.Ok(result);
        }
    }
}
