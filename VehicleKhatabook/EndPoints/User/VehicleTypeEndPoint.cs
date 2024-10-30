using FluentValidation;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class VehicleTypeEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/vehicle").WithTags("vehicle Details")/*.RequireAuthorization("OwnerOrDriverPolicy")*/;
            userRoute.MapGet("/vehicletypes", GetVehicleTypesAsync);

        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }
        internal async Task<IResult> GetVehicleTypesAsync(IMasterDataService masterDataService)
        {
            var vehicleTypes = await masterDataService.GetAllVehicleTypesAsync();
            if (!vehicleTypes.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Vehicle List"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(vehicleTypes));
        }
    }
}
