using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
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
        public async Task<List<IncomeCategory>> GetIncomeCategoriesAsync(int userTypeId)
        {
            return await _masterDataRepository.GetIncomeCategoriesAsync(userTypeId);
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

        public async Task<List<ExpenseCategory>> GetExpenseCategoriesAsync(int userTypeId)
        {
            return await _masterDataRepository.GetExpenseCategoriesAsync(userTypeId);
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
        public async Task<ApiResponse<VechileType>> AddVehicleTypesAsync(VechileTypeDTO vechileType)
        {
            return await _masterDataRepository.AddVehicleTypeAsync(vechileType);
        }
        public async Task<ApiResponse<VechileType>> UpdateVehicleTypeAsync(int vehicleTypeId, VechileTypeDTO vehicleTypeDTO)
        {
            return await _masterDataRepository.UpdateVehicleTypeAsync(vehicleTypeId, vehicleTypeDTO);
        }
        public async Task<List<VechileType>> GetAllVehicleTypesAsync()
        {
            var vehicleTypes = await _masterDataRepository.GetAllVehicleTypesAsync();

            var vehicleType = vehicleTypes.Select(vt => new VechileType
            {
                VehicleTypeId = vt.VehicleTypeId,
                TypeName = vt.TypeName,
                IsActive = vt.IsActive,
                VehicleTypeLanguageJson = vt.VehicleTypeLanguageJson,
            }).ToList();
            return vehicleType;
        }
        public async Task<List<IncomeCategoryDTO>> GetIncomeCategory()
        {
            return await _masterDataRepository.GetAllIncomeCategoryAsyc();
        }
        public async Task<List<ExpenseCategoryDTO>> GetExpenseCategory()
        {
            return await _masterDataRepository.GetAllExpenseCategoryAsyc();
        }
        public async Task<List<Country>> GetCountryAsync()
        {
            return await _masterDataRepository.GetCountryAsync();
        }


        public async Task<List<ExpenseCategory>> GetExpenseCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId)
        {
            return await _masterDataRepository.GetExpenseCategoriesForuserlanguageAsync(userTypeId, languageTypeId);
        }
        public async Task<List<IncomeCategory>> GetIncomeCategoriesForuserlanguageAsync(int userTypeId, int languageTypeId)
        {
            return await _masterDataRepository.GetIncomeCategoriesForuserlanguageAsync(userTypeId, languageTypeId);
        }
        public async Task<List<VechileType>> GetVehicleTypeForuserlanguageAsync(int languageTypeId)
        {
            return await _masterDataRepository.GetVehicleTypeForuserlanguageAsync(languageTypeId);
        }
    }
}
