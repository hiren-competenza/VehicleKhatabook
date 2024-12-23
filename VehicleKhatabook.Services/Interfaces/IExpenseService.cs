using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<UserExpense> AddExpenseAsync(ExpenseDTO expenseDTO);
        Task<UserExpense> UpdateExpenseAsync(ExpenseDTO expenseDTO, int id);
        Task<ApiResponse<UserExpense>> GetExpenseDetailsAsync(int id);
        Task<bool> DeleteExpenseAsync(int incomeExpenseId);
        Task<ApiResponse<List<UserExpense>>> GetAllExpensesAsync();
        Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId, DateTime fromDate, DateTime toDate);
        Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId);
        Task<List<UserExpense>> GetExpensebyUserAsync(Guid userId);

    }
}
