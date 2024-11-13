using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class MasterDataRepository : IMasterDataRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public MasterDataRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<List<IncomeCategory>> GetIncomeCategoriesAsync(int userTypeId)
        {
            var incomeCategories = await _context.IncomeCategories
                .Where(ic => ic.RoleId == userTypeId && ic.IsActive==true)
                .Select(ic => new IncomeCategory
                {
                    IncomeCategoryID = ic.IncomeCategoryID,
                    Name = ic.Name,
                    RoleId = ic.RoleId,
                    Description = ic.Description,
                    IsActive = ic.IsActive,
                }).ToListAsync();
            return incomeCategories;
        }

        public async Task<ApiResponse<IncomeCategory>> AddIncomeCategoryAsync(IncomeCategoryDTO categoryDTO)
        {
            var category = new IncomeCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                CreatedBy = categoryDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            _context.IncomeCategories.Add(category);
            await _context.SaveChangesAsync();
            return ApiResponse<IncomeCategory>.SuccessResponse(category , "Income category added successful.");
        }

        public async Task<ApiResponse<IncomeCategory>> UpdateIncomeCategoryAsync(int id, IncomeCategoryDTO categoryDTO)
        {
            var category = await _context.IncomeCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<IncomeCategory>.FailureResponse("Income category not found.");
            }

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.ModifiedBy = categoryDTO.ModifiedBy;
            category.LastModifiedOn = DateTime.UtcNow;
            category.IsActive = category.IsActive;

            _context.IncomeCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<IncomeCategory>.SuccessResponse(category, "Income category update successfull.");
        }

        public async Task<ApiResponse<bool>> DeleteIncomeCategoryAsync(int id)
        {
            var category = await _context.IncomeCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<bool>.FailureResponse("Income category not found.");
            }
            category.IsActive = false;
            _context.IncomeCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Income category inactive successfull.");
        }

        public async Task<List<ExpenseCategory>> GetExpenseCategoriesAsync(int userTypeId)
        {
            var expenseCategories = await _context.ExpenseCategories
                .Where(ec => ec.RoleId == userTypeId && ec.IsActive == true)
                .Select(ec => new ExpenseCategory
                {
                    ExpenseCategoryID = ec.ExpenseCategoryID,
                    Name = ec.Name,
                    RoleId = ec.RoleId,
                    Description = ec.Description,
                    IsActive = ec.IsActive,
                }).ToListAsync();
            return expenseCategories;
        }

        public async Task<ApiResponse<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategoryDTO categoryDTO)
        {
            var category = new ExpenseCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                CreatedBy = categoryDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return ApiResponse<ExpenseCategory>.SuccessResponse(category , "New Expense category added successfull.");
        }

        public async Task<ApiResponse<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategoryDTO categoryDTO)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<ExpenseCategory>.FailureResponse("Expense category not found");
            }

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.ModifiedBy = categoryDTO.ModifiedBy;
            category.LastModifiedOn = DateTime.UtcNow;
            category.IsActive = categoryDTO.IsActive;
            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<ExpenseCategory>.SuccessResponse(category, "Expense category update successfull.");
        }

        public async Task<ApiResponse<bool>> DeleteExpenseCategoryAsync(int id)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<bool>.FailureResponse("Expense category not found");
            }
            category.IsActive = false;
            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Expense category inactive successfull.");
        }
        public async Task<ApiResponse<VechileType>> AddVehicleTypeAsync(VechileType vechileType)
        {
            var vehicleType = new VechileType
            {
                TypeName = vechileType.TypeName,
                IsActive = true,
            };

            await _context.AddAsync(vehicleType);
            await _context.SaveChangesAsync();

            return ApiResponse<VechileType>.SuccessResponse(vehicleType, "Vehicle type added successfully.");
            //{
            //    Success = true,
            //    Message = "Vehicle type added successfully.",
            //    Data = new VechileType
            //    {
            //        VehicleTypeId = vehicleType.VehicleTypeId,
            //        TypeName = vehicleType.TypeName,
            //        IsActive = vechileType.IsActive
            //    }
            //};
        }


        public async Task<ApiResponse<VechileType>> UpdateVehicleTypeAsync(int vehicleTypeId, VechileType vehicleTypeDTO)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(vehicleTypeId);

            if (vehicleType == null)
            {
                return ApiResponse<VechileType>.FailureResponse("Vehicle type not found.");
            }

            vehicleType.TypeName = vehicleTypeDTO.TypeName;
            vehicleType.IsActive = vehicleTypeDTO.IsActive;
             _context.Update(vehicleType);
            await _context.SaveChangesAsync();


            return ApiResponse<VechileType>.SuccessResponse(vehicleType, "Vehicle type updated successfully.");
            //{
            //    Success = true,
            //    Message = "Vehicle type updated successfully.",
            //    Data = new VechileType
            //    {
            //        VehicleTypeId = vehicleType.VehicleTypeId,
            //        TypeName = vehicleType.TypeName
            //    }
            //};
        }
        public async Task<List<VechileType>> GetAllVehicleTypesAsync()
        {
            return await _context.VehicleTypes.ToListAsync();
        }
        public async Task<List<Country>> GetCountryAsync()
        {
            return await _context.Countries.ToListAsync();
        }
    }
}
