using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync(Guid userId);
        Task<NotificationDTO> MarkNotificationAsReadAsync(Guid notificationId);
    }
}
