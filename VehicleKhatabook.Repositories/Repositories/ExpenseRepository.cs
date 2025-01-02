using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly VehicleKhatabookDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public ExpenseRepository(VehicleKhatabookDbContext context, IUserRepository userRepository, IVehicleRepository vehicleRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<UserExpense> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
            var expense = new UserExpense
            {
                ExpenseCategoryID = expenseDTO.ExpenseCategoryID,
                ExpenseAmount = expenseDTO.ExpenseAmount,
                ExpenseDate = expenseDTO.ExpenseDate,
                ExpenseDescription = expenseDTO.ExpenseDescription,
                //UserID = expenseDTO.UserId,
                ExpenseVehicleId = expenseDTO.ExpenseVehicleId,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow
            };

            _context.UserExpenses.Add(expense);
            await _context.SaveChangesAsync();
            await _context.Entry(expense).Reference(v => v.ExpenseCategory).LoadAsync();
            await _context.Entry(expense).Reference(v => v.Vehicle).LoadAsync();
            if (expense.Vehicle != null)
            {
                await _context.Entry(expense.Vehicle).Reference(v => v.User).LoadAsync();
            }
            return expense;
        }

        public async Task<ApiResponse<UserExpense>> GetExpenseDetailsAsync(int id)
        {
            var expense = await _context.UserExpenses.FindAsync(id);
            return expense != null ? ApiResponse<UserExpense>.SuccessResponse(expense, "Expense details retrieved successfully.") : ApiResponse<UserExpense>.FailureResponse("Expense not found");
        }

        public async Task<UserExpense> UpdateExpenseAsync(ExpenseDTO expenseDTO, int id)
        {
            var expense = await _context.UserExpenses
                            .FirstOrDefaultAsync(i => i.ExpenseID == id);

            if (expense == null)
            {
                throw new KeyNotFoundException("Income record not found.");
            }
            expense.ExpenseAmount = expenseDTO.ExpenseAmount;
            expense.ExpenseDescription = expenseDTO.ExpenseDescription;
            expense.LastModifiedOn = DateTime.UtcNow;

            _context.UserExpenses.Update(expense);
            await _context.SaveChangesAsync();

            return expense;
        }


        //public async Task<ApiResponse<bool>> DeleteExpenseAsync(int id)
        //{
        //    var expense = await _context.UserExpenses.FindAsync(id);
        //    if (expense == null)
        //    {
        //        return ApiResponse<bool>.FailureResponse("Expense not found");
        //    }

        //    _context.UserExpenses.Remove(expense);
        //    await _context.SaveChangesAsync();
        //    return ApiResponse<bool>.SuccessResponse(true, "Delete successfully");
        //}

        public async Task<ApiResponse<List<UserExpense>>> GetAllExpensesAsync()
        {
            var expenses = await _context.UserExpenses.ToListAsync();
            return expenses != null ? ApiResponse<List<UserExpense>>.SuccessResponse(expenses) : ApiResponse<List<UserExpense>>.FailureResponse("Failes to get List");
        }
        public async Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId, DateTime fromDate, DateTime toDate)
        {
            Vehicle vehicle = await _vehicleRepository.GetVehicleByVehicleIdAsync(vehicleId);
            var user = await _userRepository.GetUserByIdAsync(vehicle.User.UserID);
            if (user == null)
            {
                return new List<UserExpense>(); // Return empty list if user is not found
            }

            int languageTypeId = user.LanguageType.LanguageTypeId;
            var result = await _context.UserExpenses
                .Where(e => e.ExpenseVehicleId == vehicleId && e.ExpenseDate >= fromDate && e.ExpenseDate <= toDate)
                .Include(i => i.ExpenseCategory)
                //.Include(i => i.Vehicle)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.ExpenseDate)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();

            foreach (var userExpense in result)
            {
                if (userExpense.ExpenseCategory != null &&
                    !string.IsNullOrEmpty(userExpense.ExpenseCategory.ExpenseCategoryLanguageJson)) // Check if JSON exists
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(userExpense.ExpenseCategory.ExpenseCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        userExpense.ExpenseCategory.Name = matchedLanguage.TranslatedFieldValue; // Assign translated value
                    }

                    // Optionally clear the JSON after processing
                    userExpense.ExpenseCategory.ExpenseCategoryLanguageJson = null;
                }
            }

            return result;
        }

        public async Task<List<IncomeExpenseDTO>> GetExpenseAsync(Guid vehicleId)
        {
            Vehicle vehicle = await _vehicleRepository.GetVehicleByVehicleIdAsync(vehicleId);
            var user = await _userRepository.GetUserByIdAsync(vehicle.User.UserID);
            if (user == null)
            {
                return new List<IncomeExpenseDTO>(); // Return empty list if user is not found
            }
            int languageTypeId = user.LanguageType.LanguageTypeId;

            var result = await _context.UserExpenses
                .Where(e => e.ExpenseVehicleId == vehicleId)
                .Include(i => i.ExpenseCategory)
                //.Include(i => i.Vehicle)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.ExpenseDate)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            foreach (var userExpense in result)
            {
                if (userExpense.ExpenseCategory != null &&
                    !string.IsNullOrEmpty(userExpense.ExpenseCategory.ExpenseCategoryLanguageJson)) // Check if JSON exists
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(userExpense.ExpenseCategory.ExpenseCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        userExpense.ExpenseCategory.Name = matchedLanguage.TranslatedFieldValue; // Assign translated value
                    }

                    // Optionally clear the JSON after processing
                    userExpense.ExpenseCategory.ExpenseCategoryLanguageJson = null;
                }
            }

            return result.Select(MapToDTO).ToList();
        }
        //public async Task<List<UserExpense>> GetExpensebyUserAsync(Guid userId)
        //{
        //    var result = await _context.UserExpenses
        //        .Where(e => e.Vehicle.UserID == userId)
        //        .Include(i => i.ExpenseCategory)
        //        //.Include(i => i.Vehicle)
        //        .Include(e => e.Vehicle)            // Include related Vehicle details
        //            .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
        //        .Include(e => e.Vehicle)            // Include related Vehicle details
        //            .ThenInclude(v => v.User)
        //        .OrderByDescending(i => i.ExpenseDate)
        //        .ToListAsync();
        //    return result;
        //}

        public async Task<List<IncomeExpenseDTO>> GetExpensebyUserAsync(Guid userId)
        {
            // Fetch the user's language type ID
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new List<IncomeExpenseDTO>(); // Return empty list if user is not found
            }

            int languageTypeId = user.LanguageType.LanguageTypeId;

            // Fetch user expenses
            var result = await _context.UserExpenses
                .Where(e => e.Vehicle.UserID == userId)
                .Include(i => i.ExpenseCategory)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.ExpenseDate)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();

            foreach (var userExpense in result)
            {
                if (userExpense.ExpenseCategory != null &&
                    !string.IsNullOrEmpty(userExpense.ExpenseCategory.ExpenseCategoryLanguageJson)) // Check if JSON exists
                {
                    var languageData = JsonConvert.DeserializeObject<List<MasterDataJsonLanguageDTO>>(userExpense.ExpenseCategory.ExpenseCategoryLanguageJson);

                    var matchedLanguage = languageData.FirstOrDefault(lang => lang.LanguageTypeId == languageTypeId);

                    if (matchedLanguage != null)
                    {
                        userExpense.ExpenseCategory.Name = matchedLanguage.TranslatedFieldValue; // Assign translated value
                    }

                    // Optionally clear the JSON after processing
                    userExpense.ExpenseCategory.ExpenseCategoryLanguageJson = null;
                }
            }

            return result.Select(MapToDTO).ToList();
        }
        public async Task<bool> DeleteExpenseAsync(int incomeExpenseId)
        {
            // Retrieve the income record
            var expense = await _context.UserExpenses.FindAsync(incomeExpenseId);
            if (expense == null)
            {
                return false; // Return false if the income record doesn't exist
            }

            _context.UserExpenses.Remove(expense);
            await _context.SaveChangesAsync();

            return true; // Return true if the income record was successfully deleted
        }
        private IncomeExpenseDTO MapToDTO(UserExpense userIncome)
        {
            return new IncomeExpenseDTO
            {
                CategoryID = userIncome.ExpenseCategoryID,
                Amount = userIncome.ExpenseAmount,
                Date = userIncome.ExpenseDate,
                Description = userIncome.ExpenseDescription ?? string.Empty,
                UserId = userIncome.Vehicle?.UserID ?? Guid.Empty,
                VehicleId = userIncome.ExpenseVehicleId,
                CreatedBy = userIncome.CreatedBy,
                ModifiedBy = userIncome.ModifiedBy,
                TransactionType = "debit", // Assuming this is for income transactions
                TransactionDate = userIncome.ExpenseDate,
                IncomeCategory = userIncome.ExpenseCategory != null
                    ? new IncomeCategoryDTO
                    {
                        IncomeCategoryID = userIncome.ExpenseCategory.ExpenseCategoryID,
                        Name = userIncome.ExpenseCategory.Name,
                        RoleId = userIncome.ExpenseCategory.RoleId,
                        Description = userIncome.ExpenseCategory.Description,
                        IsActive = userIncome.ExpenseCategory.IsActive,
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
