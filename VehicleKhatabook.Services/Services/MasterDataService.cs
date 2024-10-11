using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IMasterDataRepository _masterDataRepository;

        public MasterDataService(IMasterDataRepository masterDataRepository)
        {
            _masterDataRepository = masterDataRepository;
        }

        // Income Category Service Methods
        public async Task<ApiResponse<List<IncomeCategory>>> GetIncomeCategoriesAsync()
        {
            return await _masterDataRepository.GetIncomeCategoriesAsync();
        }

        public async Task<ApiResponse<IncomeCategory>> AddIncomeCategoryAsync(IncomeCategoryDTO categoryDTO)
        {
            return await _masterDataRepository.AddIncomeCategoryAsync(categoryDTO);
        }

        public async Task<ApiResponse<IncomeCategory>> UpdateIncomeCategoryAsync(int id, IncomeCategoryDTO categoryDTO)
        {
            return await _masterDataRepository.UpdateIncomeCategoryAsync(id, categoryDTO);
        }

        public async Task<ApiResponse<bool>> DeleteIncomeCategoryAsync(int id)
        {
            return await _masterDataRepository.DeleteIncomeCategoryAsync(id);
        }

        public async Task<ApiResponse<List<ExpenseCategory>>> GetExpenseCategoriesAsync()
        {
            return await _masterDataRepository.GetExpenseCategoriesAsync();
        }

        public async Task<ApiResponse<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategoryDTO categoryDTO)
        {
            return await _masterDataRepository.AddExpenseCategoryAsync(categoryDTO);
        }

        public async Task<ApiResponse<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategoryDTO categoryDTO)
        {
            return await _masterDataRepository.UpdateExpenseCategoryAsync(id, categoryDTO);
        }

        public async Task<ApiResponse<bool>> DeleteExpenseCategoryAsync(int id)
        {
            return await _masterDataRepository.DeleteExpenseCategoryAsync(id);
        }
    }
}
