using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints
{
    public class SubscriptionEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var subscriptionRoute = app.MapGroup("/api/subscription").WithTags("Subscription and Premium Features");

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
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (userId == null)
                return Results.BadRequest("User ID not found.");

            var result = await service.GetSubscriptionDetailsAsync(Guid.Parse(userId));
            if (result != null)
            {
                return Results.Ok(result);
            }
            return Results.NotFound("Subscription details not found.");
        }

        internal async Task<IResult> UpgradeToPremium(HttpContext context, ISubscriptionService service)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            if (userId == null)
                return Results.BadRequest("User ID not found.");

            var upgraded = await service.UpgradeToPremiumAsync(Guid.Parse(userId));
            if (upgraded)
            {
                return Results.Ok("Successfully upgraded to Premium.");
            }
            return Results.BadRequest("Failed to upgrade subscription.");
        }
    }
}
