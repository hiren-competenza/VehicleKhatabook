using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync(Guid userId);
        Task<Notification> MarkNotificationAsReadAsync(Guid notificationId);
    }
}
