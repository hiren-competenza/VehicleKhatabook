using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class LanguageEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var languageRoute = app.MapGroup("/api/language").WithTags("Multi-language Support").RequireAuthorization("OwnerOrDriverPolicy");

            languageRoute.MapGet("/{userId}", GetUserLanguage);
            languageRoute.MapPut("/{userId}", UpdateUserLanguage);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILanguagePreferenceRepository, LanguagePreferenceRepository>();
            services.AddScoped<ILanguagePreferenceService, LanguagePreferenceService>();
        }

        internal async Task<IResult> GetUserLanguage(Guid userId, ILanguagePreferenceService service)
        {
            var result = await service.GetUserLanguageAsync(userId);
            if (result != null)
            {
                return Results.Ok(result);
            }
            return Results.NotFound("User language preference not found.");
        }

        internal async Task<IResult> UpdateUserLanguage(Guid userId, LanguagePreferenceDTO languageDTO, ILanguagePreferenceService service)
        {
            var updated = await service.UpdateUserLanguageAsync(userId, languageDTO);
            if (updated)
            {
                return Results.Ok("Language preference updated successfully.");
            }
            return Results.BadRequest("Failed to update language preference.");
        }
    }
}
