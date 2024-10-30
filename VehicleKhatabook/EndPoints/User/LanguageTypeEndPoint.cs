using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class LanguageTypeEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/Language").WithTags("Language Details")/*.RequireAuthorization("OwnerOrDriverPolicy")*/;
            userRoute.MapGet("/GetAllLanguageTypes", GetAllLanguageTypes);

        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILanguageTypeService, LanguageTypeService>();
            services.AddScoped<ILanguageTypeRepository, LanguageTypeRepository>();
        }
        public async Task<IResult> GetAllLanguageTypes(ILanguageTypeService languageTypeService)
        {
            var result = await languageTypeService.GetAllLanguageTypesAsync();
            if (!result.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Language List"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result));
        }
    }
}
