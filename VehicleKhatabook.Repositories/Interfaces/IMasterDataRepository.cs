using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IMasterDataRepository
    {
        Task<List<IncomeCategory>> GetIncomeCategoriesAsync(int userTypeId);
        Task<ApiResponse<IncomeCategory>> AddIncomeCategoryAsync(IncomeCategoryDTO categoryDTO);
        Task<ApiResponse<IncomeCategory>> UpdateIncomeCategoryAsync(int id, IncomeCategoryDTO categoryDTO);
        Task<ApiResponse<bool>> DeleteIncomeCategoryAsync(int id);

        Task<List<ExpenseCategory>> GetExpenseCategoriesAsync(int userTypeId);
        Task<ApiResponse<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategoryDTO categoryDTO);
        Task<ApiResponse<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategoryDTO categoryDTO);
        Task<ApiResponse<bool>> DeleteExpenseCategoryAsync(int id);
        Task<ApiResponse<VechileType>> AddVehicleTypeAsync(VechileTypeDTO vechileType);
        Task<ApiResponse<VechileType>> UpdateVehicleTypeAsync(int vehicleTypeId, VechileTypeDTO vehicleTypeDTO);
        Task<List<VechileType>> GetAllVehicleTypesAsync();
        Task<List<Country>> GetCountryAsync();
        Task<List<IncomeCategoryDTO>> GetAllIncomeCategoryAsyc();
        Task<List<ExpenseCategoryDTO>> GetAllExpenseCategoryAsyc();

        Task<List<ExpenseCategory>> GetExpenseCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId);
        Task<List<IncomeCategory>> GetIncomeCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId);
        Task<List<VechileType>> GetVehicleTypeForuserlanguageAsync(int languageTypeId);
        Task<List<ApplicationConfiguration>> GetApplicationConfiguration();
        Task<ApiResponse<ApplicationConfiguration>> AddApplicationConfiguration(ApplicationConfiguration ConfigurationDTO);
        Task<ApiResponse<ApplicationConfiguration>> UpdateApplicationConfiguration(Guid userId, ApplicationConfiguration ConfigurationDTO);
        Task<List<State>> GetStateAsync(int id);
        Task<List<District>> GetDistrictAsync(int id);
        Task<ApiResponse<PaymentHistory>> AddPaymentRecord(PaymentHistory paymentHistory);
        Task<List<PaymentHistory>> GetAllPaymentRecord();
        Task<List<SubscriptionMaster>> GetSubscriptionMasterAsync();
        Task<List<PaymentHistory>> GetAllPaymentRecordByUserId(string userId);
        //Task<ApiResponse<bool>> DeletePaymentRecordById(string id);


    }
}