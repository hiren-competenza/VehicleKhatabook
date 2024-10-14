using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public ExpenseRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Expense>> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
            var expense = new Expense
            {
                VehicleID = expenseDTO.VehicleID,
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

        public async Task<ApiResponse<Expense>> GetExpenseDetailsAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            return expense != null ? new ApiResponse<Expense> { Success = true, Data = expense } : new ApiResponse<Expense> { Success = false, Message = "Expense not found" };
        }

        public async Task<ApiResponse<Expense>> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return new ApiResponse<Expense> { Success = false, Message = "Expense not found" };
            }

            expense.ExpenseAmount = expenseDTO.ExpenseAmount;
            expense.ExpenseCategoryID = expenseDTO.ExpenseCategoryID;
            expense.ExpenseDate = expenseDTO.ExpenseDate;
            expense.ExpenseDescription = expenseDTO.ExpenseDescription;
            //expense.ModifiedBy = expenseDTO.ModifiedBy;
            expense.LastModifiedOn = DateTime.UtcNow;

            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return new ApiResponse<Expense> { Success = true, Data = expense };
        }

        public async Task<ApiResponse<bool>> DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return new ApiResponse<bool> { Success = false, Message = "Expense not found" };
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Data = true };
        }

        public async Task<ApiResponse<List<Expense>>> GetAllExpensesAsync()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return new ApiResponse<List<Expense>> { Success = true, Data = expenses };
        }
    }
}
