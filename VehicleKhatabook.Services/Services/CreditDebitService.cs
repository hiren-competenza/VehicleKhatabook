using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class CreditDebitService : ICreditDebitService
    {
        private readonly ICreditDebitRepositories _creditDebitRepositories;

        public CreditDebitService(ICreditDebitRepositories creditDebitRepositories)
        {
            _creditDebitRepositories = creditDebitRepositories;
        }
        public async Task<ApiResponse<Expense>> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
            return await _creditDebitRepositories.AddExpenseAsync(expenseDTO);
        }
        public async Task<ApiResponse<Income>> AddIncomeAsync(IncomeDTO incomeDTO)
        {
            return await _creditDebitRepositories.AddIncomeAsync(incomeDTO);
        }
        public async Task<ApiResponse<List<Expense>>> GetExpenseAsync(Guid userId)
        {
            return await _creditDebitRepositories.GetExpenseAsync(userId);
        }
        public async Task<ApiResponse<List<Income>>> GetIncomeAsync(Guid userId)
        {
            return await _creditDebitRepositories.GetIncomeAsync(userId);
        }
    }
}
