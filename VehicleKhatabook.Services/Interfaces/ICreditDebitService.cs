using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface ICreditDebitService
    {
        Task<ApiResponse<Expense>> AddExpenseAsync(ExpenseDTO expenseDTO);
        Task<ApiResponse<Income>> AddIncomeAsync(IncomeDTO incomeDTO);
        Task<ApiResponse<List<Expense>>> GetExpenseAsync(Guid userId);
        Task<ApiResponse<List<Income>>> GetIncomeAsync(Guid userId);
    }
}
