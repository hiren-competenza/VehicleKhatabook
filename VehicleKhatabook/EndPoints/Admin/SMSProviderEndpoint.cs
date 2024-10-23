using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class SMSProviderEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api").WithTags("SMSProvider Management");
            staticRoute.MapGet("/GetAllSMSProvider", GetAllSMSProviders);
            staticRoute.MapPost("/AddSMSProvider", AddSMSProvider);
            staticRoute.MapPut("/UpdateSMSProvider", UpdateSMSProvider);
        }


        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISMSProviderService, SMSProviderService>();
            services.AddScoped<ISMSProviderRepository, SMSProviderRepository>();
        }

        internal async Task<IResult> GetAllSMSProviders(ISMSProviderService sMSProviderService)
        {
            var result = await sMSProviderService.GetAllSMSProvidersAsync();
            return Results.Ok(result);
        }
        internal async Task<IResult> AddSMSProvider(SMSProviderDTO smsProviderDTO, ISMSProviderService sMSProviderService)
        {
            var result = await sMSProviderService.AddSMSProviderAsync(smsProviderDTO);
            return Results.Ok(result);
        }
        internal async Task<IResult> UpdateSMSProvider(int id,  SMSProviderDTO smsProviderDTO,  ISMSProviderService sMSProviderService)
        {
            var result = await sMSProviderService.UpdateSMSProviderAsync(id, smsProviderDTO);
            return Results.Ok(result);
           
        }
    }
}
