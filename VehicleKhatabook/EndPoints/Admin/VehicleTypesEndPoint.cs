using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Infrastructure;
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
            var staticRoute = app.MapGroup("/api/static").WithTags("Vehicle Types Management")/*.RequireAuthorization("AdminPolicy")*/;
            staticRoute.MapPost("/api/AddVehicleTypes", AddVehicleTypesAsync);
            staticRoute.MapPost("/api/UpdateVehicleType", UpdateVehicleTypeAsync);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        internal async Task<IResult> AddVehicleTypesAsync(VechileType vechileType, IMasterDataService masterDataService)
        {
            var vehicleTypes = await masterDataService.AddVehicleTypesAsync(vechileType);
            return Results.Ok(vehicleTypes);
        }
        internal async Task<IResult> UpdateVehicleTypeAsync(int vehicleTypeId, VechileType vehicleTypeDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateVehicleTypeAsync(vehicleTypeId, vehicleTypeDTO);
            return Results.Ok(result);
        }
    }
}
