using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface ILanguageTypeRepository
    {
        Task<List<LanguageTypeDTO>> GetAllLanguageTypesAsync();
        Task<LanguageTypeDTO?> AddLanguageTypeAsync(LanguageTypeDTO languageTypeDTO);
        Task<LanguageTypeDTO?> UpdateLanguageTypeAsync(int id, LanguageTypeDTO languageTypeDTO);
    }

}
