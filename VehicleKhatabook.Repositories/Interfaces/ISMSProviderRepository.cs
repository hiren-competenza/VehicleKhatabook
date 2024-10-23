using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface ISMSProviderRepository
    {
        Task<List<SMSProviderDTO>> GetAllSMSProvidersAsync();
        Task<SMSProviderDTO?> AddSMSProviderAsync(SMSProviderDTO smsProviderDTO);
        Task<SMSProviderDTO?> UpdateSMSProviderAsync(int id, SMSProviderDTO smsProviderDTO);
    }

}
