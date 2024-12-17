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
            staticRoute.MapPatch("/updateApplicationConfiguration/{id}", UpdateApplicationConfiguration);
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
            var config = response?.FirstOrDefault();

            if (config == null)
            {
                return Results.BadRequest("No configuration found.");
            }
            var configuration = config.GetConfigurationFields(config)
                .Select(field => new
                {
                    FieldName = field.FieldName,
                    Label = field.Label,
                    Value = field.Value
                });
            var result = configuration;

            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }



    }
}
