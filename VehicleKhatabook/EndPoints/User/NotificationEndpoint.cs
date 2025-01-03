using AutoMapper;
using System.Security.Claims;
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

            notifications.MapGet("/", GetAllNotificationsUserId);
            notifications.MapGet("/GetAllNotifications", GetAllNotifications);
            notifications.MapPost("/mark-read/{id}", MarkNotificationAsRead);
            notifications.MapDelete("/deleteall", DeleteAllNotifications);
            notifications.MapDelete("/delete", DeleteAllNotificationsForCurrentUser);
        }

        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
        }

        private async Task<IResult> GetAllNotificationsUserId(HttpContext httpContext,INotificationService notificationService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }
            var notifications = await notificationService.GetAllNotificationsAsync(Guid.Parse(userId));
            if (notifications == null)
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("No notification found"));
            }
            return Results.Ok(ApiResponse<object>.SuccessResponse(notifications, "Notification"));
        }
        private async Task<IResult> GetAllNotifications(HttpContext httpContext, INotificationService notificationService)
        {
            
            var notifications = await notificationService.GetAllNotifications();
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
        private async Task<IResult> DeleteAllNotifications(INotificationService notificationService)
        {
            await notificationService.DeleteAllNotificationsAsync();
            return Results.Ok(ApiResponse<object>.SuccessResponse(null, "All notifications have been deleted successfully."));
        }

        // DELETE: Delete all notifications for the current user
        private async Task<IResult> DeleteAllNotificationsForCurrentUser(HttpContext httpContext, INotificationService notificationService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Ok(ApiResponse<object>.FailureResponse("User not found."));
            }

            await notificationService.DeleteAllNotificationsForUserAsync(Guid.Parse(userId));
            return Results.Ok(ApiResponse<object>.SuccessResponse(null, "All notifications for the current user have been deleted successfully."));
        }
    }
}
