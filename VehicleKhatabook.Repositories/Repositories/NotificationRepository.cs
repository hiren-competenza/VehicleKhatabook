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
                .Where(n => n.UserID == userId)
                .ToListAsync();
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
    }

}
