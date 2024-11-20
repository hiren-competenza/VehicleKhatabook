using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IMasterDataService
    {
        Task<List<IncomeCategory>> GetIncomeCategoriesAsync(int userTypeId);
        Task<ApiResponse<IncomeCategory>> AddIncomeCategoryAsync(IncomeCategoryDTO categoryDTO);
        Task<ApiResponse<IncomeCategory>> UpdateIncomeCategoryAsync(int id, IncomeCategoryDTO categoryDTO);
        Task<ApiResponse<bool>> DeleteIncomeCategoryAsync(int id);

        Task<List<ExpenseCategory>> GetExpenseCategoriesAsync(int userTypeId);
        Task<ApiResponse<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategoryDTO categoryDTO);
        Task<ApiResponse<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategoryDTO categoryDTO);
        Task<ApiResponse<bool>> DeleteExpenseCategoryAsync(int id);
        Task<ApiResponse<VechileType>> AddVehicleTypesAsync(VechileTypeDTO vechileType);
        Task<ApiResponse<VechileType>> UpdateVehicleTypeAsync(int vehicleTypeId, VechileTypeDTO vehicleTypeDTO);
        Task<List<VechileType>> GetAllVehicleTypesAsync();
        Task<List<Country>> GetCountryAsync();
    }
}
