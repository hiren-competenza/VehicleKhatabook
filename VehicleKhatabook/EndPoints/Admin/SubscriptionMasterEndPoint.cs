using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.Admin
{
    public class SubscriptionMasterEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var staticRoute = app.MapGroup("/api/master").WithTags("SubscriptionMasterEndPoint")/*.RequireAuthorization("AdminPolicy")*/;

            staticRoute.MapGet("/GetSubscriptionMaster", GetSubscriptionMasterAsync);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }


        internal async Task<IResult> GetSubscriptionMasterAsync(IMasterDataService masterDataService)
        {
            var SusbscriptionType = await masterDataService.GetSubscriptionMasterAsync();
            if (!SusbscriptionType.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Vehicle List"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(SusbscriptionType));
        }
    }
}
