using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public NotificationRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(n => n.UserID == userId) // Filter by UserID
                .OrderByDescending(n => n.NotificationDate) // Optional: Order notifications by date
                .ToListAsync(); // Convert to list asynchronously
        }


        public async Task<Notification> MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.LastModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return notification!;
        }
        public async Task AddNotificationsAsync(IEnumerable<Notification> notifications)
        {
            if (notifications == null || !notifications.Any())
            {
                return; // No notifications to add
            }

            // Add notifications in bulk
            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task DeleteAllNotificationsAsync()
        {
            var notifications = _context.Notifications;
            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllNotificationsForUserAsync(Guid userId)
        {
            var notifications = _context.Notifications.Where(n => n.UserID == userId);
            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
        }
    }
}
