using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<Expense> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
            return await _expenseRepository.AddExpenseAsync(expenseDTO);
        }

        public async Task<ApiResponse<Expense>> GetExpenseDetailsAsync(int id)
        {
            return await _expenseRepository.GetExpenseDetailsAsync(id);
        }

        public async Task<ApiResponse<Expense>> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO)
        {
            return await _expenseRepository.UpdateExpenseAsync(id, expenseDTO);
        }

        public async Task<ApiResponse<bool>> DeleteExpenseAsync(int id)
        {
            return await _expenseRepository.DeleteExpenseAsync(id);
        }

        public async Task<ApiResponse<List<Expense>>> GetAllExpensesAsync()
        {
            return await _expenseRepository.GetAllExpensesAsync();
        }
        public async Task<List<Expense>> GetExpenseAsync(Guid userId)
        {
            return await _expenseRepository.GetExpenseAsync(userId);
        }
    }
}
