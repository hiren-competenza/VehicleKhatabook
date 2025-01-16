using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class PaymentRecordsEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var subscriptionRoute = app.MapGroup("/api/payment").WithTags("Payment Records Endpoint").RequireAuthorization("OwnerOrDriverPolicy");

            subscriptionRoute.MapPost("/AddRecords", AddRecordsAsync);
            subscriptionRoute.MapGet("/GetAllRecords", GetAllRecordsAsync);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
        }
       internal async Task<IResult> AddRecordsAsync(string? transactionId,decimal? amount, int? packageId,string? status, int? validity, HttpContext context, IMasterDataService masterDataService)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var records = await masterDataService.AddRecordsAsync(transactionId, status,(decimal)amount, packageId, (int)validity,Guid.Parse(userId));

            return Results.Ok(ApiResponse<object>.SuccessResponse(records, "Successfully add the records"));
        }
        internal async Task<IResult> GetAllRecordsAsync(IMasterDataService masterDataService)
        {
            var records = await masterDataService.GetAllRecordsAsync();

            if (!records.Any())
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Not Found Any Payment Record"));
            }

            return Results.Ok(ApiResponse<object>.SuccessResponse(records));
        }

    }
}
