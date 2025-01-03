﻿using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync(Guid userId);
        Task<IEnumerable<Notification>> GetAllNotifications();
        Task<Notification> MarkNotificationAsReadAsync(Guid notificationId);
        Task AddNotificationsAsync(IEnumerable<Notification> notifications);
        Task DeleteAllNotificationsAsync();
        Task DeleteAllNotificationsForUserAsync(Guid userId);
    }
}
