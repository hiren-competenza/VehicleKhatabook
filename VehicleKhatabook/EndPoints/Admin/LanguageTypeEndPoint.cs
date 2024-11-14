using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class LanguageTypeEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/master").WithTags("Language Type Management");//.RequireAuthorization("AdminPolicy");
            staticRoute.MapPost("/addLanguageType", AddLanguageType);
            staticRoute.MapPut("/updateLanguageType", UpdateLanguageType);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILanguageTypeService, LanguageTypeService>();
            services.AddScoped<ILanguageTypeRepository, LanguageTypeRepository>();
        }

        public async Task<IResult> AddLanguageType(LanguageTypeDTO languageTypeDTO, ILanguageTypeService languageTypeService)
        {
            var result = await languageTypeService.AddLanguageTypeAsync(languageTypeDTO);
            if (result.status == 200)
            {
                return Results.Ok(result);
            }
            return Results.BadRequest(result.Message);
        }

        public async Task<IResult> UpdateLanguageType(int id, LanguageTypeDTO languageTypeDTO, ILanguageTypeService languageTypeService)
        {
            var result = await languageTypeService.UpdateLanguageTypeAsync(id, languageTypeDTO);
            if (result.status == 200)
            {
                return Results.Ok(result);
            }
            return Results.BadRequest(result.Message);
        }
    }
}
