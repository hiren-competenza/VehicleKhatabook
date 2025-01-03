using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync(Guid userId);
        Task<IEnumerable<Notification>> GetAllNotifications();
        Task<NotificationDTO> MarkNotificationAsReadAsync(Guid notificationId);
        Task AddNotificationsAsync(IEnumerable<Notification> notifications);
        Task CheckForExpirationsAndNotifyAsync();
        Task DeleteAllNotificationsAsync();
        Task DeleteAllNotificationsForUserAsync(Guid userId);
    }
}
