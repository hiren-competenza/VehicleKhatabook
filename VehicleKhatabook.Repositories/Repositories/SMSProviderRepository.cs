using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class SMSProviderRepository : ISMSProviderRepository
    {
        private readonly VehicleKhatabookDbContext _dbContext;

        public SMSProviderRepository(VehicleKhatabookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SMSProviderDTO>> GetAllSMSProvidersAsync()
        {
            return await _dbContext.SMSProviderConfigs
                                   .Select(p => new SMSProviderDTO
                                   {
                                       ProviderID = p.ProviderID,
                                       ProviderName = p.ProviderName,
                                       APIUrl = p.APIUrl,
                                       AuthKey = p.AuthKey,
                                       ClientID = p.ClientID,
                                       SenderID = p.SenderID,
                                       Timeout = p.Timeout,
                                       IsActive = p.IsActive
                                   }).ToListAsync();
        }

        public async Task<SMSProviderDTO?> AddSMSProviderAsync(SMSProviderDTO smsProviderDTO)
        {
            var smsProvider = new SMSProviderConfig
            {
                ProviderName = smsProviderDTO.ProviderName,
                APIUrl = smsProviderDTO.APIUrl,
                AuthKey = smsProviderDTO.AuthKey,
                ClientID = smsProviderDTO.ClientID,
                SenderID = smsProviderDTO.SenderID,
                Timeout = smsProviderDTO.Timeout,
                IsActive = smsProviderDTO.IsActive,
                CreatedBy = smsProviderDTO.CreatedBy,
                CreatedOn = DateTime.Now
            };

            _dbContext.SMSProviderConfigs.Add(smsProvider);
            await _dbContext.SaveChangesAsync();

            smsProviderDTO.ProviderID = smsProvider.ProviderID;
            return smsProviderDTO;
        }

        public async Task<SMSProviderDTO?> UpdateSMSProviderAsync(int id, SMSProviderDTO smsProviderDTO)
        {
            var smsProvider = await _dbContext.SMSProviderConfigs.FindAsync(id);
            if (smsProvider == null)
            {
                return null;
            }

            smsProvider.ProviderName = smsProviderDTO.ProviderName;
            smsProvider.APIUrl = smsProviderDTO.APIUrl;
            smsProvider.AuthKey = smsProviderDTO.AuthKey;
            smsProvider.ClientID = smsProviderDTO.ClientID;
            smsProvider.SenderID = smsProviderDTO.SenderID;
            smsProvider.Timeout = smsProviderDTO.Timeout;
            smsProvider.IsActive = smsProviderDTO.IsActive;
            smsProvider.ModifiedBy = smsProviderDTO.ModifiedBy;
            smsProvider.LastModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return smsProviderDTO;
        }
    }

}
