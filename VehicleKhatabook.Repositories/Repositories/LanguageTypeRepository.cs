using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class LanguageTypeRepository : ILanguageTypeRepository
    {
        private readonly VehicleKhatabookDbContext _dbContext;

        public LanguageTypeRepository(VehicleKhatabookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LanguageTypeDTO>> GetAllLanguageTypesAsync()
        {
            return await _dbContext.LanguageTypes
                                   .Select(lt => new LanguageTypeDTO
                                   {
                                       Description = lt.Description,
                                       LanguageTypeId = lt.LanguageTypeId,
                                       LanguageName = lt.LanguageName,
                                       IsActive = lt.IsActive
                                   }).ToListAsync();
        }

        public async Task<LanguageTypeDTO?> AddLanguageTypeAsync(LanguageTypeDTO languageTypeDTO)
        {
            var languageType = new LanguageType
            {
                LanguageName = languageTypeDTO.LanguageName,
                Description = languageTypeDTO.Description,
                IsActive = true
            };

            _dbContext.LanguageTypes.Add(languageType);
            await _dbContext.SaveChangesAsync();

            languageTypeDTO.LanguageTypeId = languageType.LanguageTypeId;
            return languageTypeDTO;
        }

        public async Task<LanguageTypeDTO?> UpdateLanguageTypeAsync(int id, LanguageTypeDTO languageTypeDTO)
        {
            var languageType = await _dbContext.LanguageTypes.FindAsync(id);
            if (languageType == null)
            {
                return null;
            }

            languageType.LanguageName = languageTypeDTO.LanguageName;
            languageType.Description = languageTypeDTO.Description;
            languageType.IsActive = languageTypeDTO.IsActive;

            await _dbContext.SaveChangesAsync();
            return languageTypeDTO;
        }
    }

}
