using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class LanguagePreferenceService : ILanguagePreferenceService
    {
        private readonly ILanguagePreferenceRepository _repository;

        public LanguagePreferenceService(ILanguagePreferenceRepository repository)
        {
            _repository = repository;
        }

        public async Task<LanguagePreferenceDTO> GetUserLanguageAsync(Guid userId)
        {
            var preference = await _repository.GetUserLanguageAsync(userId);
            if (preference != null)
            {
                return new LanguagePreferenceDTO
                {
                    UserId = preference.UserId,
                    LanguageCode = preference.LanguageCode
                };
            }
            return null!;
        }

        public async Task<bool> UpdateUserLanguageAsync(Guid userId, LanguagePreferenceDTO languagePreferenceDTO)
        {
            var languagePreference = await _repository.GetUserLanguageAsync(userId);
            if (languagePreference == null) return false;

            languagePreference.LanguageCode = languagePreferenceDTO.LanguageCode;
            languagePreference.LastModifiedOn = DateTime.UtcNow;

            return await _repository.UpdateUserLanguageAsync(languagePreference);
        }
    }
}
