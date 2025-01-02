using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly VehicleKhatabookDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public IncomeRepository(VehicleKhatabookDbContext context, IUserRepository userRepository, IVehicleRepository vehicleRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<UserIncome> AddIncomeAsync(IncomeDTO incomeDTO)
        {
            var income = new UserIncome
            {
                IncomeCategoryID = incomeDTO.IncomeCategoryID,
                IncomeAmount = incomeDTO.IncomeAmount,
                IncomeDate = incomeDTO.IncomeDate,
                //UserID = incomeDTO.UserId,
                IncomeDescription = incomeDTO.IncomeDescription,
                IncomeVehicleId = incomeDTO.IncomeVehicleId,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            _context.UserIncomes.Add(income);
            await _context.SaveChangesAsync();
            await _context.Entry(income).Reference(v => v.IncomeCategory).LoadAsync();
            await _context.Entry(income).Reference(v => v.Vehicle).LoadAsync();
            if (income.Vehicle != null)
            {
                await _context.Entry(income.Vehicle).Reference(v => v.User).LoadAsync();
            }

            // Return the updated vehicle with related data
            return income;
        }

        public async Task<ApiResponse<UserIncome>> GetIncomeDetailsAsync(int id)
        {
            var income = await _context.UserIncomes.FindAsync(id);
            return income != null ? ApiResponse<UserIncome>.SuccessResponse(income) : ApiResponse<UserIncome>.FailureResponse("Income not found");
        }

        //public async Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO)
        //{
        //    var income = await _context.Incomes.FindAsync(id);
        //    if (income == null)
        //    {
        //        return new ApiResponse<Income> { Success = false, Message = "Income not found" };
        //    }

        //    income.IncomeAmount = incomeDTO.IncomeAmount;
        //    income.IncomeCategoryID = incomeDTO.IncomeCategoryID;
        //    income.IncomeDate = incomeDTO.IncomeDate;
        //    //income.ModifiedBy = incomeDTO.ModifiedBy;
        //    income.LastModifiedOn = DateTime.UtcNow;

        //    _context.Incomes.Update(income);
        //    await _context.SaveChangesAsync();
        //    return new ApiResponse<Income> { Success = true, Data = income };
        //}

        //public async Task<ApiResponse<bool>> DeleteIncomeAsync(int id)
        //{
        //    var income = await _context.Incomes.FindAsync(id);
        //    if (income == null)
        //    {
        //        return new ApiResponse<bool> { Success = false, Message = "Income not found" };
        //    }

        //    _context.Incomes.Remove(income);
        //    await _context.SaveChangesAsync();
        //    return new ApiResponse<bool> { Success = true, Data = true };
        //}

        //public async Task<ApiResponse<List<Income>>> GetAllIncomesAsync()
        //{
        //    var incomes = await _context.Incomes.ToListAsync();
        //    return new ApiResponse<List<Income>> { Success = true, Data = incomes };
        //}
        //public async Task<List<UserIncome>> GetIncomeAsync(Guid userId, int months)
        //{
        //    var startDate = DateTime.UtcNow.AddMonths(-months);
        //    var result = await _context.UserIncomes
        //        .Where(i => i.UserID == userId && i.IncomeDate >= startDate)
        //        .ToListAsync();
        //    return result;
        //}
        public async Task<List<UserIncome>> GetIncomeAsync(Guid vehicleId, DateTime fromDate, DateTime toDate)
        {
            Vehicle vehicle = await _vehicleRepository.GetVehicleByVehicleIdAsync(vehicleId);
            var user = await _userRepository.GetUserByIdAsync(vehicle.User.UserID);
            if (user == null)
            {
                return new List<UserIncome>(); // Return empty list if user is not found
            }

            int languageTypeId = user.LanguageType.LanguageTypeId;
            var result = await _context.UserIncomes
                .Where(i => i.IncomeVehicleId == vehicleId && i.IncomeDate >= fromDate && i.IncomeDate <= toDate)
                .Include(i => i.IncomeCategory)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.IncomeDate)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            foreach (var userIncome in result)
            {
                if (userIncome.IncomeCategory != null &&
                    !string.IsNullOrEmpty(userIncome.IncomeCategory.IncomeCategoryLanguageJson)) // Check JSON is not null or empty
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(userIncome.IncomeCategory.IncomeCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        userIncome.IncomeCategory.Name = matchedLanguage.TranslatedFieldValue; // Assign translated value to Name
                    }

                    // Optionally clear the JSON after processing
                    userIncome.IncomeCategory.IncomeCategoryLanguageJson = null;
                }
            }
            return result;
        }
        public async Task<List<IncomeExpenseDTO>> GetIncomeAsync(Guid vehicleId)
        {
            Vehicle vehicle = await _vehicleRepository.GetVehicleByVehicleIdAsync(vehicleId);
            var user = await _userRepository.GetUserByIdAsync(vehicle.User.UserID);
            if (user == null)
            {
                return new List<IncomeExpenseDTO>(); // Return empty list if user is not found
            }

            int languageTypeId = user.LanguageType.LanguageTypeId;
            var result = await _context.UserIncomes
                .Where(i => i.IncomeVehicleId == vehicleId)
                .Include(i => i.IncomeCategory)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.IncomeDate)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            foreach (var userIncome in result)
            {
                if (userIncome.IncomeCategory != null &&
                    !string.IsNullOrEmpty(userIncome.IncomeCategory.IncomeCategoryLanguageJson)) // Check JSON is not null or empty
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(userIncome.IncomeCategory.IncomeCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        userIncome.IncomeCategory.Name = matchedLanguage.TranslatedFieldValue; // Assign translated value to Name
                    }

                    // Optionally clear the JSON after processing
                    userIncome.IncomeCategory.IncomeCategoryLanguageJson = null;
                }
            }
            return result.Select(MapToDTO).ToList();
        }
        //public async Task<List<UserIncome>> GetIncomebyUserAsync(Guid userId)
        //{
        //    var result = await _context.UserIncomes
        //        .Where(i => i.Vehicle.UserID == userId)
        //        .Include(i => i.IncomeCategory)
        //        .Include(e => e.Vehicle)            // Include related Vehicle details
        //            .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
        //        .Include(e => e.Vehicle)            // Include related Vehicle details
        //            .ThenInclude(v => v.User)
        //        .OrderByDescending(i => i.IncomeDate)
        //        .ToListAsync();
        //    return result;
        //}
        public async Task<List<IncomeExpenseDTO>> GetIncomebyUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new List<IncomeExpenseDTO>(); // Return empty list if user is not found
            }

            int languageTypeId = user.LanguageType.LanguageTypeId;
            var result = await _context.UserIncomes
                .Where(i => i.Vehicle.UserID == userId)
                .Include(i => i.IncomeCategory)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.IncomeDate)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();

            foreach (var userIncome in result)
            {
                if (userIncome.IncomeCategory != null &&
                    !string.IsNullOrEmpty(userIncome.IncomeCategory.IncomeCategoryLanguageJson)) // Check JSON is not null or empty
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(userIncome.IncomeCategory.IncomeCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        userIncome.IncomeCategory.Name = matchedLanguage.TranslatedFieldValue; // Assign translated value to Name
                    }

                    // Optionally clear the JSON after processing
                    userIncome.IncomeCategory.IncomeCategoryLanguageJson = null;
                }
            }

            return result.Select(MapToDTO).ToList();
        }

        public async Task<UserIncome> UpdateIncomeAsync(IncomeDTO incomeDTO, int incomeExpenseId)
        {
            var income = await _context.UserIncomes
                .FirstOrDefaultAsync(i => i.IncomeID == incomeExpenseId);

            if (income == null)
            {
                throw new KeyNotFoundException("Income record not found.");
            }

            income.IncomeAmount = incomeDTO.IncomeAmount;
            income.LastModifiedOn = DateTime.UtcNow;
            income.IncomeDescription = incomeDTO.IncomeDescription;

            _context.UserIncomes.Update(income);
            await _context.SaveChangesAsync();

            return income;
        }

        public async Task<bool> DeleteIncomeAsync(int incomeExpenseId)
        {
            var income = await _context.UserIncomes.FindAsync(incomeExpenseId);
            if (income == null)
            {
                return false;
            }

            _context.UserIncomes.Remove(income);
            await _context.SaveChangesAsync();

            return true;
        }

        private IncomeExpenseDTO MapToDTO(UserIncome userIncome)
        {
            return new IncomeExpenseDTO
            {
                CategoryID = userIncome.IncomeCategoryID,
                Amount = userIncome.IncomeAmount,
                Date = userIncome.IncomeDate,
                Description = userIncome.IncomeDescription ?? string.Empty,
                UserId = userIncome.Vehicle?.UserID ?? Guid.Empty,
                VehicleId = userIncome.IncomeVehicleId,
                CreatedBy = userIncome.CreatedBy,
                ModifiedBy = userIncome.ModifiedBy,
                TransactionType = "credit", // Assuming this is for income transactions
                TransactionDate = userIncome.IncomeDate,
                IncomeCategory = userIncome.IncomeCategory != null
                    ? new IncomeCategoryDTO
                    {
                        IncomeCategoryID = userIncome.IncomeCategory.IncomeCategoryID,
                        Name = userIncome.IncomeCategory.Name,
                        RoleId = userIncome.IncomeCategory.RoleId,
                        Description = userIncome.IncomeCategory.Description,
                        IsActive = userIncome.IncomeCategory.IsActive,
                    }
                    : null, // If IncomeCategory is null, set DTO to null
                Vehicle = userIncome.Vehicle != null
                    ? new VehicleDTO
                    {
                        UserId = userIncome.Vehicle.UserID,
                        VehicleTypeId = userIncome.Vehicle.VehicleTypeId,
                        RegistrationNumber = userIncome.Vehicle.RegistrationNumber,
                        NickName = userIncome.Vehicle.NickName,
                        InsuranceExpiry = userIncome.Vehicle.InsuranceExpiry,
                        PollutionExpiry = userIncome.Vehicle.PollutionExpiry,
                        FitnessExpiry = userIncome.Vehicle.FitnessExpiry,
                        RoadTaxExpiry = userIncome.Vehicle.RoadTaxExpiry,
                        RCPermitExpiry = userIncome.Vehicle.RCPermitExpiry,
                        NationalPermitExpiry = userIncome.Vehicle.NationalPermitExpiry,
                        ChassisNumber = userIncome.Vehicle.ChassisNumber,
                        EngineNumber = userIncome.Vehicle.EngineNumber,
                        IsActive = userIncome.Vehicle.IsActive
                    }
                    : null // If Vehicle is null, set DTO to null
            };
        }
    }
}
