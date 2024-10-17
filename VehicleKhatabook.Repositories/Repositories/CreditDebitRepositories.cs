using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class CreditDebitRepositories : ICreditDebitRepositories
    {
        private readonly VehicleKhatabookDbContext _context;
        public CreditDebitRepositories(VehicleKhatabookDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<Income>> AddIncomeAsync(IncomeDTO incomeDTO)
        {
             var IsIncomeCategory = await _context.IncomeCategories.AnyAsync(ic => ic.IncomeCategoryID == incomeDTO.IncomeCategoryID);
            if (!IsIncomeCategory)
            {
                return new ApiResponse<Income>
                {
                    Success = false,
                    Message = $"Income category with ID {incomeDTO.IncomeCategoryID} does not exist."
                };
            }
            var income = new Income
            {
                IncomeCategoryID = incomeDTO.IncomeCategoryID,
                IncomeAmount = incomeDTO.IncomeAmount,
                IncomeDate = incomeDTO.IncomeDate,
                DriverID = incomeDTO.DriverID,
                IncomeDescription = incomeDTO.IncomeDescription,
                //CreatedBy = incomeDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return new ApiResponse<Income> { Success = true, Data = income };
        }
        public async Task<ApiResponse<Expense>> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
            var expenseCategory = await _context.ExpenseCategories.AnyAsync(ec => ec.ExpenseCategoryID == expenseDTO.ExpenseCategoryID);

            if (!expenseCategory)
            {
                return new ApiResponse<Expense>
                {
                    Success = false,
                    Message = $"Expense category with ID {expenseDTO.ExpenseCategoryID} does not exist."
                };
            }

            var expense = new Expense
            {
                ExpenseCategoryID = expenseDTO.ExpenseCategoryID,
                ExpenseAmount = expenseDTO.ExpenseAmount,
                ExpenseDate = expenseDTO.ExpenseDate,
                ExpenseDescription = expenseDTO.ExpenseDescription,
                DriverID = expenseDTO.DriverID,
                //CreatedBy = expenseDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return new ApiResponse<Expense> { Success = true, Data = expense };
        }

        public async Task<ApiResponse<List<Income>>> GetIncomeAsync(Guid userId)
        {
            var result = await _context.Incomes
                .Where(i => i.DriverID == userId)
                .ToListAsync();

            if (result == null || result.Count == 0)
            {
                return new ApiResponse<List<Income>>
                {
                    Success = false,
                    Message = $"No income records found for user ID {userId}."
                };
            }

            return new ApiResponse<List<Income>>
            {
                Success = true,
                Data = result
            };
        }
        public async Task<ApiResponse<List<Expense>>> GetExpenseAsync(Guid userId)
        {
            var result = await _context.Expenses
                .Where(e => e.DriverID == userId)
                .ToListAsync();

            if (result == null || result.Count == 0)
            {
                return new ApiResponse<List<Expense>>
                {
                    Success = false,
                    Message = $"No expense records found for user ID {userId}."
                };
            }

            return new ApiResponse<List<Expense>>
            {
                Success = true,
                Data = result
            };
        }
    }
}
