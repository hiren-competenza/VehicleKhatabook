using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<UserExpense> AddExpenseAsync(ExpenseDTO expenseDTO);
        Task<ApiResponse<UserExpense>> GetExpenseDetailsAsync(int id);
        Task<UserExpense> UpdateExpenseAsync(ExpenseDTO expenseDTO ,int id);
        //Task<ApiResponse<bool>> DeleteExpenseAsync(int id);
        Task<ApiResponse<List<UserExpense>>> GetAllExpensesAsync();
        Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId, DateTime fromDate, DateTime toDate);
        Task<List<IncomeExpenseDTO>> GetExpenseAsync(Guid vehicleId);
        Task<List<IncomeExpenseDTO>> GetExpensebyUserAsync(Guid userId);
        Task<bool> DeleteExpenseAsync(int incomeExpenseId);
    }
}
