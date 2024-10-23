using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class LanguageTypeService : ILanguageTypeService
    {
        private readonly ILanguageTypeRepository _languageTypeRepository;

        public LanguageTypeService(ILanguageTypeRepository languageTypeRepository)
        {
            _languageTypeRepository = languageTypeRepository;
        }

        public async Task<ApiResponse<List<LanguageTypeDTO>>> GetAllLanguageTypesAsync()
        {
            var languageTypes = await _languageTypeRepository.GetAllLanguageTypesAsync();
            return languageTypes.Count != 0 ? ApiResponse<List<LanguageTypeDTO>>.SuccessResponse(languageTypes) : ApiResponse<List<LanguageTypeDTO>>.FailureResponse("Not Found any Data");
        }

        public async Task<ApiResponse<LanguageTypeDTO>> AddLanguageTypeAsync(LanguageTypeDTO languageTypeDTO)
        {
            var result = await _languageTypeRepository.AddLanguageTypeAsync(languageTypeDTO);
            return result != null
                ? ApiResponse<LanguageTypeDTO>.SuccessResponse(result, "Language type added successfully.")
                : ApiResponse<LanguageTypeDTO>.FailureResponse("Failed to add language type.");
        }

        public async Task<ApiResponse<LanguageTypeDTO>> UpdateLanguageTypeAsync(int id, LanguageTypeDTO languageTypeDTO)
        {
            var result = await _languageTypeRepository.UpdateLanguageTypeAsync(id, languageTypeDTO);
            return result != null
                ? ApiResponse<LanguageTypeDTO>.SuccessResponse(result, "Language type updated successfully.")
                : ApiResponse<LanguageTypeDTO>.FailureResponse("Failed to update language type.");
        }
    }

}
