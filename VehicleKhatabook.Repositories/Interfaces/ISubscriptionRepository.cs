using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetSubscriptionDetailsAsync(Guid userId);
        Task<bool> UpdateSubscriptionAsync(Subscription subscription);
    }
}
