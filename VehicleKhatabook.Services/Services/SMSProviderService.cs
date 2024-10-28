using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class SMSProviderService : ISMSProviderService
    {
        private readonly ISMSProviderRepository _smsProviderRepository;

        public SMSProviderService(ISMSProviderRepository smsProviderRepository)
        {
            _smsProviderRepository = smsProviderRepository;
        }

        public async Task<List<SMSProviderDTO>> GetAllSMSProvidersAsync()
        {
            return await _smsProviderRepository.GetAllSMSProvidersAsync();
        }

        public async Task<ApiResponse<SMSProviderDTO>> AddSMSProviderAsync(SMSProviderDTO smsProviderDTO)
        {
            var result = await _smsProviderRepository.AddSMSProviderAsync(smsProviderDTO);
            return result != null
                ? ApiResponse<SMSProviderDTO>.SuccessResponse(result, "SMS provider added successfully.")
                : ApiResponse<SMSProviderDTO>.FailureResponse("Failed to add SMS provider.");
        }

        public async Task<ApiResponse<SMSProviderDTO>> UpdateSMSProviderAsync(int id, SMSProviderDTO smsProviderDTO)
        {
            var result = await _smsProviderRepository.UpdateSMSProviderAsync(id, smsProviderDTO);
            return result != null
                ? ApiResponse<SMSProviderDTO>.SuccessResponse(result, "SMS provider updated successfully.")
                : ApiResponse<SMSProviderDTO>.FailureResponse("Failed to update SMS provider.");
        }
    }

}
