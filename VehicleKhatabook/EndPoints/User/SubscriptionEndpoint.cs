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
    public class SubscriptionEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var subscriptionRoute = app.MapGroup("/api/subscription").WithTags("Subscription and Premium Features")/*.RequireAuthorization("OwnerOrDriverPolicy")*/;

            subscriptionRoute.MapGet("/details", GetSubscriptionDetails);
            subscriptionRoute.MapPost("/upgrade", UpgradeToPremium);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
        }

        internal async Task<IResult> GetSubscriptionDetails(HttpContext context, ISubscriptionService service)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var result = await service.GetSubscriptionDetailsAsync(Guid.Parse(userId));
            if (result == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Subscription details not found."));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(result, "User is already subscribed"));
        }

        internal async Task<IResult> UpgradeToPremium(HttpContext context, ISubscriptionService service)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            var upgraded = await service.UpgradeToPremiumAsync(Guid.Parse(userId));
            if (!upgraded)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Failed to upgrade subscription."));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(true, "Successfully upgraded to Premium."));
        }
    }
}
