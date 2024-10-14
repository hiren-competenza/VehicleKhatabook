using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _repository;

        public SubscriptionService(ISubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<SubscriptionDTO?> GetSubscriptionDetailsAsync(Guid userId)
        {
            var subscription = await _repository.GetSubscriptionDetailsAsync(userId);
            if (subscription != null)
            {
                return new SubscriptionDTO
                {
                    SubscriptionType = subscription.SubscriptionType,
                    SubscriptionStartDate = subscription.SubscriptionStartDate,
                    SubscriptionEndDate = subscription.SubscriptionEndDate
                };
            }
            return null;
        }

        public async Task<bool> UpgradeToPremiumAsync(Guid userId)
        {
            var subscription = await _repository.GetSubscriptionDetailsAsync(userId);
            if (subscription == null) return false;

            subscription.SubscriptionType = "Premium";
            subscription.SubscriptionStartDate = DateTime.UtcNow;
            subscription.SubscriptionEndDate = DateTime.UtcNow.AddYears(1);

            return await _repository.UpdateSubscriptionAsync(subscription);
        }
    }
}
