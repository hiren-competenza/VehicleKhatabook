using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface ILanguagePreferenceService
    {
        Task<LanguagePreferenceDTO> GetUserLanguageAsync(Guid userId);
        Task<bool> UpdateUserLanguageAsync(Guid userId, LanguagePreferenceDTO languagePreferenceDTO);
    }
}
