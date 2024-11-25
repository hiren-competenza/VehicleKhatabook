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

        public async Task<UserExpense> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
            return await _expenseRepository.AddExpenseAsync(expenseDTO);
        }

        public async Task<ApiResponse<UserExpense>> GetExpenseDetailsAsync(int id)
        {
            return await _expenseRepository.GetExpenseDetailsAsync(id);
        }

        public async Task<ApiResponse<UserExpense>> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO)
        {
            return await _expenseRepository.UpdateExpenseAsync(id, expenseDTO);
        }

        public async Task<ApiResponse<bool>> DeleteExpenseAsync(int id)
        {
            return await _expenseRepository.DeleteExpenseAsync(id);
        }

        public async Task<ApiResponse<List<UserExpense>>> GetAllExpensesAsync()
        {
            return await _expenseRepository.GetAllExpensesAsync();
        }
        public async Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId, DateTime fromDate, DateTime toDate)
        {
            return await _expenseRepository.GetExpenseAsync(vehicleId, fromDate, toDate);
        }
        public async Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId)
        {
            return await _expenseRepository.GetExpenseAsync(vehicleId);
        }
        public async Task<List<UserExpense>> GetExpensebyUserAsync(Guid userId)
        {
            return await _expenseRepository.GetExpensebyUserAsync(userId);
        }
    }
}
