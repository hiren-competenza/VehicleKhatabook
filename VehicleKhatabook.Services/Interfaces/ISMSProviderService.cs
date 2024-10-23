using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface ISMSProviderService
    {
        Task<ApiResponse<List<SMSProviderDTO>>> GetAllSMSProvidersAsync();
        Task<ApiResponse<SMSProviderDTO>> AddSMSProviderAsync(SMSProviderDTO smsProviderDTO);
        Task<ApiResponse<SMSProviderDTO>> UpdateSMSProviderAsync(int id, SMSProviderDTO smsProviderDTO);
    }

}
