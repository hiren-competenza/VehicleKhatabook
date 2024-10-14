using VehicleKhatabook.Entities.Models;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface ILanguagePreferenceRepository
    {
        Task<LanguagePreference?> GetUserLanguageAsync(Guid userId);
        Task<bool> UpdateUserLanguageAsync(LanguagePreference languagePreference);
    }
}
