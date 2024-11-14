using AutoMapper;
using VehicleKhatabook.Infrastructure;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;
using VehicleKhatabook.Services.Services;

namespace VehicleKhatabook.EndPoints.User
{
    public class NotificationEndpoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var notifications = app.MapGroup("/api/notifications").WithTags("Notifications & Alerts")/*.RequireAuthorization("OwnerOrDriverPolicy")*/;

            notifications.MapGet("/", GetAllNotifications);
            notifications.MapPost("/mark-read/{id}", MarkNotificationAsRead);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
        }

        private async Task<IResult> GetAllNotifications(Guid userId, INotificationService notificationService)
        {
            var notifications = await notificationService.GetAllNotificationsAsync(userId);
            if (notifications == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("No notification found"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(notifications, "Notification"));
        }

        private async Task<IResult> MarkNotificationAsRead(Guid id, INotificationService notificationService)
        {
            var notification = await notificationService.MarkNotificationAsReadAsync(id);
            if (notification == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("Notification not found."));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(notification, "Notification read"));
        }
    }
}
