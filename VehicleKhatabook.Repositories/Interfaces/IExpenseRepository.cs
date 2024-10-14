using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<ApiResponse<Expense>> AddExpenseAsync(ExpenseDTO expenseDTO);
        Task<ApiResponse<Expense>> GetExpenseDetailsAsync(int id);
        Task<ApiResponse<Expense>> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO);
        Task<ApiResponse<bool>> DeleteExpenseAsync(int id);
        Task<ApiResponse<List<Expense>>> GetAllExpensesAsync();
    }
}
