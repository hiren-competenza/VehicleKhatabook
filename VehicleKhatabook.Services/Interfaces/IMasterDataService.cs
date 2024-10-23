using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IMasterDataService
    {
        Task<ApiResponse<List<IncomeCategory>>> GetIncomeCategoriesAsync(int userTypeId);
        Task<ApiResponse<IncomeCategory>> AddIncomeCategoryAsync(IncomeCategoryDTO categoryDTO);
        Task<ApiResponse<IncomeCategory>> UpdateIncomeCategoryAsync(int id, IncomeCategoryDTO categoryDTO);
        Task<ApiResponse<bool>> DeleteIncomeCategoryAsync(int id);

        Task<ApiResponse<List<ExpenseCategory>>> GetExpenseCategoriesAsync(int userTypeId);
        Task<ApiResponse<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategoryDTO categoryDTO);
        Task<ApiResponse<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategoryDTO categoryDTO);
        Task<ApiResponse<bool>> DeleteExpenseCategoryAsync(int id);
        Task<ApiResponse<VechileType>> AddVehicleTypesAsync(VechileType vechileType);
        Task<ApiResponse<VechileType>> UpdateVehicleTypeAsync(int vehicleTypeId, VechileType vehicleTypeDTO);
        Task<ApiResponse<List<VechileType>>> GetAllVehicleTypesAsync();
        Task<ApiResponse<List<Country>>> GetCountryAsync();
    }
}
