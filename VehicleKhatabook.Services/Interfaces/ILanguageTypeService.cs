using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface ILanguageTypeService
    {
        Task<ApiResponse<List<LanguageTypeDTO>>> GetAllLanguageTypesAsync();
        Task<ApiResponse<LanguageTypeDTO>> AddLanguageTypeAsync(LanguageTypeDTO languageTypeDTO);
        Task<ApiResponse<LanguageTypeDTO>> UpdateLanguageTypeAsync(int id, LanguageTypeDTO languageTypeDTO);
    }

}
