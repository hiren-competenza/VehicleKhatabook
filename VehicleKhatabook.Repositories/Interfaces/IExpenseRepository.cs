using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<UserExpense> AddExpenseAsync(ExpenseDTO expenseDTO);
        Task<ApiResponse<UserExpense>> GetExpenseDetailsAsync(int id);
        Task<ApiResponse<UserExpense>> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO);
        Task<ApiResponse<bool>> DeleteExpenseAsync(int id);
        Task<ApiResponse<List<UserExpense>>> GetAllExpensesAsync();
        Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId, DateTime fromDate, DateTime toDate);
    }
}
