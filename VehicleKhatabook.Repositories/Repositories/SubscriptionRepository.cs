using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public SubscriptionRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription?> GetSubscriptionDetailsAsync(Guid userId)
        {
            return await _context.Subscriptions
                .FirstOrDefaultAsync(sub => sub.UserID == userId);
        }

        public async Task<bool> UpdateSubscriptionAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
