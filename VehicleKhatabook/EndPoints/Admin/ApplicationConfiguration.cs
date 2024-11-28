using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;
using VehicleKhatabook.Entities.Models;


namespace VehicleKhatabook.EndPoints.Admin
{
    public class ApplicationConfigurations : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/master").WithTags("Application Configuration Management");//.RequireAuthorization("AdminPolicy");
            staticRoute.MapPost("/addApplicationConfiguration", AddApplicationConfiguration);
            staticRoute.MapPut("/updateApplicationConfiguration/{id}", UpdateApplicationConfiguration);
            staticRoute.MapGet("/GetApplicationConfiguration", GetApplicationConfiguration);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }

        internal async Task<IResult> AddApplicationConfiguration(ApplicationConfiguration ConfigurationDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.AddApplicationConfiguration(ConfigurationDTO);
            return Results.Ok(result);
        }

        internal async Task<IResult> UpdateApplicationConfiguration(Guid id, ApplicationConfiguration ConfigurationDTO, IMasterDataService masterDataService)
        {
            var result = await masterDataService.UpdateApplicationConfiguration(id, ConfigurationDTO);
            return Results.Ok(result);
        }

     
        internal async Task<IResult> GetApplicationConfiguration(IMasterDataService masterDataService)
        {

            var response = await masterDataService.GetApplicationConfiguration();

            return Results.Ok(ApiResponse<object>.SuccessResponse(response));

        }
    }
}
