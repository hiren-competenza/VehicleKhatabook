using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDTO?> GetSubscriptionDetailsAsync(Guid userId);
        Task<bool> UpgradeToPremiumAsync(Guid userId);
    }
}
