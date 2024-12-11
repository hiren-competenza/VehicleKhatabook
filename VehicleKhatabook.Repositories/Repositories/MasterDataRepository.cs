using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public MasterDataRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<List<IncomeCategory>> GetIncomeCategoriesAsync(int userTypeId)
        {
            var incomeCategories = await _context.IncomeCategories
                .Where(ic => ic.RoleId == userTypeId && ic.IsActive == true)
                .Select(ic => new IncomeCategory
                {
                    IncomeCategoryID = ic.IncomeCategoryID,
                    Name = ic.Name,
                    RoleId = ic.RoleId,
                    Description = ic.Description,
                    IsActive = ic.IsActive,
                }).ToListAsync();
            return incomeCategories;
        }

        public async Task<ApiResponse<IncomeCategory>> AddIncomeCategoryAsync(IncomeCategoryDTO categoryDTO)
        {
            var category = new IncomeCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = categoryDTO.ModifiedBy,
                IsActive = true,
                RoleId = categoryDTO.RoleId,
                IncomeCategoryID = categoryDTO.IncomeCategoryID,
                IncomeCategoryLanguageJson = categoryDTO.IncomeCategoryLanguageJson
            };

            _context.IncomeCategories.Add(category);
            await _context.SaveChangesAsync();
            return ApiResponse<IncomeCategory>.SuccessResponse(category, "Income category added successful.");
        }

        public async Task<ApiResponse<IncomeCategory>> UpdateIncomeCategoryAsync(int id, IncomeCategoryDTO categoryDTO)
        {
            var category = await _context.IncomeCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<IncomeCategory>.FailureResponse("Income category not found.");
            }

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.IncomeCategoryLanguageJson = categoryDTO.IncomeCategoryLanguageJson;
            category.RoleId = categoryDTO.RoleId;
            category.ModifiedBy = 1;
            category.LastModifiedOn = DateTime.UtcNow;
            category.IsActive = category.IsActive;

            _context.IncomeCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<IncomeCategory>.SuccessResponse(category, "Income category update successfull.");
        }

        public async Task<ApiResponse<bool>> DeleteIncomeCategoryAsync(int id)
        {
            var category = await _context.IncomeCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<bool>.FailureResponse("Income category not found.");
            }
            category.IsActive = false;
            _context.IncomeCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Income category inactive successfull.");
        }

        public async Task<List<ExpenseCategory>> GetExpenseCategoriesAsync(int userTypeId)
        {
            var expenseCategories = await _context.ExpenseCategories
                .Where(ec => ec.RoleId == userTypeId && ec.IsActive == true)
                .Select(ec => new ExpenseCategory
                {
                    ExpenseCategoryID = ec.ExpenseCategoryID,
                    Name = ec.Name,
                    RoleId = ec.RoleId,
                    Description = ec.Description,
                    IsActive = ec.IsActive,
                }).ToListAsync();
            return expenseCategories;
        }

        public async Task<ApiResponse<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategoryDTO categoryDTO)
        {
            var category = new ExpenseCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                ExpenseCategoryLanguageJson = categoryDTO.ExpenseCategoryLanguageJson,
                RoleId = categoryDTO.RoleId,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow,
                IsActive = true,
                ExpenseCategoryID = categoryDTO.ExpenseCategoryID,

            };

            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return ApiResponse<ExpenseCategory>.SuccessResponse(category, "New Expense category added successfull.");
        }

        public async Task<ApiResponse<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategoryDTO categoryDTO)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<ExpenseCategory>.FailureResponse("Expense category not found");
            }

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.ExpenseCategoryLanguageJson = categoryDTO.ExpenseCategoryLanguageJson;
            category.ModifiedBy = 1;
            category.RoleId = categoryDTO.RoleId;
            category.LastModifiedOn = DateTime.UtcNow;
            category.IsActive = categoryDTO.IsActive;
            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<ExpenseCategory>.SuccessResponse(category, "Expense category update successfull.");
        }

        public async Task<ApiResponse<bool>> DeleteExpenseCategoryAsync(int id)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<bool>.FailureResponse("Expense category not found");
            }
            category.IsActive = false;
            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Expense category inactive successfull.");
        }
        public async Task<ApiResponse<VechileType>> AddVehicleTypeAsync(VechileTypeDTO vechileTypeDTO)
        {
            var vehicleType = new VechileType
            {
                TypeName = vechileTypeDTO.TypeName,
                Description = vechileTypeDTO.Description,
                VehicleTypeLanguageJson = vechileTypeDTO.VehicleTypeLanguageJson,
                IsActive = true,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow,
            };

            await _context.AddAsync(vehicleType);
            await _context.SaveChangesAsync();

            return ApiResponse<VechileType>.SuccessResponse(vehicleType, "Vehicle type added successfully.");
            //{
            //    Success = true,
            //    Message = "Vehicle type added successfully.",
            //    Data = new VechileType
            //    {
            //        VehicleTypeId = vehicleType.VehicleTypeId,
            //        TypeName = vehicleType.TypeName,
            //        IsActive = vechileType.IsActive
            //    }
            //};
        }

        public async Task<ApiResponse<VechileType>> UpdateVehicleTypeAsync(int vehicleTypeId, VechileTypeDTO vehicleTypeDTO)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(vehicleTypeId);

            if (vehicleType == null)
            {
                return ApiResponse<VechileType>.FailureResponse("Vehicle type not found.");
            }

            vehicleType.TypeName = vehicleTypeDTO.TypeName;
            vehicleType.IsActive = vehicleTypeDTO.IsActive;
            vehicleType.Description = vehicleTypeDTO.Description;
            vehicleType.VehicleTypeLanguageJson = vehicleTypeDTO.VehicleTypeLanguageJson;
            vehicleType.ModifiedBy = 1;
            vehicleType.LastModifiedOn = DateTime.UtcNow;
            _context.Update(vehicleType);
            await _context.SaveChangesAsync();


            return ApiResponse<VechileType>.SuccessResponse(vehicleType, "Vehicle type updated successfully.");
            //{
            //    Success = true,
            //    Message = "Vehicle type updated successfully.",
            //    Data = new VechileType
            //    {
            //        VehicleTypeId = vehicleType.VehicleTypeId,
            //        TypeName = vehicleType.TypeName
            //    }
            //};
        }
        public async Task<List<VechileType>> GetAllVehicleTypesAsync()
        {
            return await _context.VehicleTypes.ToListAsync();
        }
        public async Task<List<IncomeCategoryDTO>> GetAllIncomeCategoryAsyc()
        {
            var income = _context.IncomeCategories.ToList();

            var incomeCategory = income.Select(vt => new IncomeCategoryDTO
            {
                IncomeCategoryID = vt.IncomeCategoryID,
                Name = vt.Name,
                IncomeCategoryLanguageJson = vt.IncomeCategoryLanguageJson,
                Description = vt.Description,
                IsActive = (bool)vt.IsActive,
                RoleId = (int)vt.RoleId,

            }).ToList();
            return incomeCategory;


        }
        public async Task<List<ExpenseCategoryDTO>> GetAllExpenseCategoryAsyc()
        {
            var expense = _context.ExpenseCategories.ToList();

            var expenseCategory = expense.Select(vt => new ExpenseCategoryDTO
            {
                ExpenseCategoryID = vt.ExpenseCategoryID,
                Name = vt.Name,
                Description = vt.Description,
                IsActive = (bool)vt.IsActive,
                RoleId = (int)vt.RoleId,
                ExpenseCategoryLanguageJson = vt.ExpenseCategoryLanguageJson,

            }).ToList();
            return expenseCategory;


        }
        public async Task<List<Country>> GetCountryAsync()
        {
            return await _context.Countries.ToListAsync();
        }
        

        public async Task<List<ExpenseCategory>> GetExpenseCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId)
        {
            var result = await _context.ExpenseCategories
                .Where(ec => ec.RoleId == userTypeId)  // Filter by RoleId
                .ToListAsync();

            foreach (var expenseCategory in result)
            {
                if (!string.IsNullOrEmpty(expenseCategory.ExpenseCategoryLanguageJson))  // Check if the JSON is not null or empty
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(expenseCategory.ExpenseCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        expenseCategory.Name = matchedLanguage.TranslatedFieldValue;  // Assign the translated language to the Name field
                    }

                    // Optionally, clear the JSON field after processing
                    expenseCategory.ExpenseCategoryLanguageJson = null;
                }
            }

            return result;
        }

        public async Task<List<IncomeCategory>> GetIncomeCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId)
        {
            var result = await _context.IncomeCategories
                .Where(ec => ec.RoleId == userTypeId)  // Filter by RoleId
                .ToListAsync();

            foreach (var incomeCategory in result)
            {
                if (!string.IsNullOrEmpty(incomeCategory.IncomeCategoryLanguageJson))  // Check if the JSON is not null or empty
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(incomeCategory.IncomeCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        incomeCategory.Name = matchedLanguage.TranslatedFieldValue;  // Assign the translated language to the Name field
                    }

                    // Optionally, clear the JSON field after processing
                    incomeCategory.IncomeCategoryLanguageJson = null;
                }
            }

            return result;
        }

        public async Task<List<VechileType>> GetVehicleTypeForuserlanguageAsync(int languageTypeId)
        {
            var result = await _context.VehicleTypes.ToListAsync();

            foreach (var vehicleType in result)
            {
                if (!string.IsNullOrEmpty(vehicleType.VehicleTypeLanguageJson)) // Check if JSON is not null or empty
                {
                    // Deserialize the JSON field into a dynamic list
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(vehicleType.VehicleTypeLanguageJson);

                    // Find the matched language by LanguageTypeId
                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        // Assign the translated language to the TypeName field
                        vehicleType.TypeName = matchedLanguage.TranslatedFieldValue;
                    }

                    // Optionally, set VehicleTypeLanguageJson to null to exclude it from the result
                    vehicleType.VehicleTypeLanguageJson = null;
                }
            }
            return result;
        }

        public async Task<List<ApplicationConfiguration>> GetApplicationConfiguration()
        {
            return await _context.ApplicationConfigurations.ToListAsync();
        }



        public async Task<ApiResponse<ApplicationConfiguration>> AddApplicationConfiguration(ApplicationConfiguration configurationDTO)
        {
            
                var configuration = new ApplicationConfiguration
                {
                    SMSApiKey = configurationDTO.SMSApiKey,
                    SMSApiUrl = configurationDTO.SMSApiUrl,
                    SMSSenderId = configurationDTO.SMSSenderId,
                    SupportEmail = configurationDTO.SupportEmail,
                    SupportWhatsAppNumber = configurationDTO.SupportWhatsAppNumber,
                    PaymentGatewayApiKey = configurationDTO.PaymentGatewayApiKey,
                    PaymentGatewayPublicKey = configurationDTO.PaymentGatewayPublicKey,
                    PaymentGatewayWebhookSecret = configurationDTO.PaymentGatewayWebhookSecret,
                    PaymentGatewayCurrency = configurationDTO.PaymentGatewayCurrency,
                    PaymentGatewayName = configurationDTO.PaymentGatewayName,
                    PaymentGatewayStatus = configurationDTO.PaymentGatewayStatus,
                    SubscriptionName = configurationDTO.SubscriptionName,
                    SubscriptionAmount = configurationDTO.SubscriptionAmount,
                    SubscriptionDurationDays = configurationDTO.SubscriptionDurationDays,
                    IsRenewable = configurationDTO.IsRenewable,
                    RenewalReminderDaysBefore = configurationDTO.RenewalReminderDaysBefore,
                    TrialPeriodDays = configurationDTO.TrialPeriodDays,
                    FacebookPageUrl = configurationDTO.FacebookPageUrl,
                    TwitterHandle = configurationDTO.TwitterHandle,
                    InstagramHandle = configurationDTO.InstagramHandle,
                    LinkedInUrl = configurationDTO.LinkedInUrl,
                    YouTubeChannelUrl = configurationDTO.YouTubeChannelUrl,
                    PinterestUrl = configurationDTO.PinterestUrl,
                    IsActive = configurationDTO.IsActive,

                    CreatedBy = configurationDTO.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };

                _context.ApplicationConfigurations.Add(configuration);
                await _context.SaveChangesAsync();

                return ApiResponse<ApplicationConfiguration>.SuccessResponse(configuration, "Configuration added successfully.");
            
            
        }


        public async Task<ApiResponse<ApplicationConfiguration>> UpdateApplicationConfiguration(Guid configurationId, ApplicationConfiguration configurationDTO)
        {
            try
            {
                var existingConfig = await _context.ApplicationConfigurations.FirstOrDefaultAsync(c => c.ApplicationConfigurationId == configurationId);

                if (existingConfig == null)
                {
                    return ApiResponse<ApplicationConfiguration>.FailureResponse("Configuration not found.");
                }

                // Update properties
                existingConfig.SMSApiKey = configurationDTO.SMSApiKey;
                existingConfig.SMSApiUrl = configurationDTO.SMSApiUrl;
                existingConfig.SMSSenderId = configurationDTO.SMSSenderId;
                existingConfig.SupportEmail = configurationDTO.SupportEmail;
                existingConfig.SupportWhatsAppNumber = configurationDTO.SupportWhatsAppNumber;
                existingConfig.PaymentGatewayApiKey = configurationDTO.PaymentGatewayApiKey;
                existingConfig.PaymentGatewayPublicKey = configurationDTO.PaymentGatewayPublicKey;
                existingConfig.PaymentGatewayWebhookSecret = configurationDTO.PaymentGatewayWebhookSecret;
                existingConfig.PaymentGatewayCurrency = configurationDTO.PaymentGatewayCurrency;
                existingConfig.PaymentGatewayName = configurationDTO.PaymentGatewayName;
                existingConfig.PaymentGatewayStatus = configurationDTO.PaymentGatewayStatus;
                existingConfig.SubscriptionName = configurationDTO.SubscriptionName;
                existingConfig.SubscriptionAmount = configurationDTO.SubscriptionAmount;
                existingConfig.SubscriptionDurationDays = configurationDTO.SubscriptionDurationDays;
                existingConfig.IsRenewable = configurationDTO.IsRenewable;
                existingConfig.RenewalReminderDaysBefore = configurationDTO.RenewalReminderDaysBefore;
                existingConfig.TrialPeriodDays = configurationDTO.TrialPeriodDays;
                existingConfig.FacebookPageUrl = configurationDTO.FacebookPageUrl;
                existingConfig.TwitterHandle = configurationDTO.TwitterHandle;
                existingConfig.InstagramHandle = configurationDTO.InstagramHandle;
                existingConfig.LinkedInUrl = configurationDTO.LinkedInUrl;
                existingConfig.YouTubeChannelUrl = configurationDTO.YouTubeChannelUrl;
                existingConfig.PinterestUrl = configurationDTO.PinterestUrl;
                existingConfig.IsActive = configurationDTO.IsActive;

                existingConfig.LastModifiedOn = DateTime.UtcNow;

                _context.ApplicationConfigurations.Update(existingConfig);
                await _context.SaveChangesAsync();

                return ApiResponse<ApplicationConfiguration>.SuccessResponse(existingConfig, "Configuration updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ApplicationConfiguration>.FailureResponse($"An error occurred while updating the configuration: {ex.Message}");
            }
        }

        public Task<List<State>> GetStateAsync(int id)
        {
            return _context.State.Include(x => x.Country)
                .Where(s => s.CountryId == id)
                .Select(s => new State
                {
                    Id = s.Id,
                    CountryId = s.CountryId,
                    StateName = s.StateName,
                    IsActive = s.IsActive,
                    CreatedOn = s.CreatedOn,
                    Country = s.Country,
                })
                .ToListAsync();
        }

    }
}
