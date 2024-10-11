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

        public async Task<ApiResponse<List<IncomeCategory>>> GetIncomeCategoriesAsync()
        {
            var categories = await _context.IncomeCategories.ToListAsync();
            return new ApiResponse<List<IncomeCategory>> { Success = true, Data = categories };
        }

        public async Task<ApiResponse<IncomeCategory>> AddIncomeCategoryAsync(IncomeCategoryDTO categoryDTO)
        {
            var category = new IncomeCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                CreatedBy = categoryDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.IncomeCategories.Add(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<IncomeCategory> { Success = true, Data = category };
        }

        public async Task<ApiResponse<IncomeCategory>> UpdateIncomeCategoryAsync(int id, IncomeCategoryDTO categoryDTO)
        {
            var category = await _context.IncomeCategories.FindAsync(id);
            if (category == null)
            {
                return new ApiResponse<IncomeCategory> { Success = false, Message = "Income category not found" };
            }

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.ModifiedBy = categoryDTO.ModifiedBy;
            category.LastModifiedOn = DateTime.UtcNow;

            _context.IncomeCategories.Update(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<IncomeCategory> { Success = true, Data = category };
        }

        public async Task<ApiResponse<bool>> DeleteIncomeCategoryAsync(int id)
        {
            var category = await _context.IncomeCategories.FindAsync(id);
            if (category == null)
            {
                return new ApiResponse<bool> { Success = false, Message = "Income category not found" };
            }

            _context.IncomeCategories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Data = true };
        }

        public async Task<ApiResponse<List<ExpenseCategory>>> GetExpenseCategoriesAsync()
        {
            var categories = await _context.ExpenseCategories.ToListAsync();
            return new ApiResponse<List<ExpenseCategory>> { Success = true, Data = categories };
        }

        public async Task<ApiResponse<ExpenseCategory>> AddExpenseCategoryAsync(ExpenseCategoryDTO categoryDTO)
        {
            var category = new ExpenseCategory
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                CreatedBy = categoryDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<ExpenseCategory> { Success = true, Data = category };
        }

        public async Task<ApiResponse<ExpenseCategory>> UpdateExpenseCategoryAsync(int id, ExpenseCategoryDTO categoryDTO)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category == null)
            {
                return new ApiResponse<ExpenseCategory> { Success = false, Message = "Expense category not found" };
            }

            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.ModifiedBy = categoryDTO.ModifiedBy;
            category.LastModifiedOn = DateTime.UtcNow;

            _context.ExpenseCategories.Update(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<ExpenseCategory> { Success = true, Data = category };
        }

        public async Task<ApiResponse<bool>> DeleteExpenseCategoryAsync(int id)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category == null)
            {
                return new ApiResponse<bool> { Success = false, Message = "Expense category not found" };
            }

            _context.ExpenseCategories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Data = true };
        }
    }
}
