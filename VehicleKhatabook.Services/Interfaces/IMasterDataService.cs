﻿using VehicleKhatabook.Entities.Models;
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
        Task<List<IncomeCategoryDTO>> GetIncomeCategory();
        Task<List<ExpenseCategoryDTO>> GetExpenseCategory();
        Task<List<Country>> GetCountryAsync();

        Task<List<ExpenseCategory>> GetExpenseCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId);
        Task<List<IncomeCategory>> GetIncomeCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId);
        Task<List<VechileType>> GetVehicleTypeForuserlanguageAsync(int languageTypeId);
        Task<List<ApplicationConfiguration>> GetApplicationConfiguration();
        Task<ApiResponse<ApplicationConfiguration>> AddApplicationConfiguration(ApplicationConfiguration ConfigurationDTO);
        Task<ApiResponse<ApplicationConfiguration>> UpdateApplicationConfiguration(Guid userId, ApplicationConfiguration ConfigurationDTO);
        Task<List<State>> GetStateAsync(int Id);
        Task<List<District>> GetDistrictAsync(int Id);
        Task<ApiResponse<PaymentHistory>> AddPaymentRecord(PaymentHistory paymentHistory);
        Task<ApiResponse<PaymentHistory>> AddRecordsAsync(string? transactionId,string? status,decimal? amount,int? packageId,int? validity, Guid? userId);
        Task<List<PaymentHistory>> GetAllPaymentRecord();
        Task<List<SubscriptionMaster>> GetSubscriptionMasterAsync();
        Task<ApiResponse<SubscriptionMaster>> AddSubscriptionMasterAsync(SubscriptionMasterDTO subscriptionMasterDTO);
        Task<List<PaymentHistory>> GetAllPaymentRecordByUserId(string UserId);
        Task<List<PaymentHistory>> GetAllRecordsAsync();
        //Task<ApiResponse<bool>> DeletePaymentRecordById(string PayementId);

    }
}
