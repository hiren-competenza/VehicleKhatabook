using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class LanguagePreferenceRepository : ILanguagePreferenceRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public LanguagePreferenceRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<LanguagePreference?> GetUserLanguageAsync(Guid userId)
        {
            return await _context.LanguagePreferences
                .FirstOrDefaultAsync(lp => lp.UserId == userId);
        }

        public async Task<bool> UpdateUserLanguageAsync(LanguagePreference languagePreference)
        {
            _context.LanguagePreferences.Update(languagePreference);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
