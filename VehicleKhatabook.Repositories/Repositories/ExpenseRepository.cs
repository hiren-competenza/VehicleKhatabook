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

        public async Task<Expense> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
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
            return expense;
        }

        public async Task<ApiResponse<Expense>> GetExpenseDetailsAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            return expense != null ? ApiResponse<Expense>.SuccessResponse(expense, "Expense details retrieved successfully.")  : ApiResponse<Expense>.FailureResponse("Expense not found");
        }

        public async Task<ApiResponse<Expense>> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return ApiResponse<Expense>.FailureResponse($"Unable to update/expense for for id{id} Not Found");
            }

            expense.ExpenseAmount = expenseDTO.ExpenseAmount;
            expense.ExpenseCategoryID = expenseDTO.ExpenseCategoryID;
            expense.ExpenseDate = expenseDTO.ExpenseDate;
            expense.ExpenseDescription = expenseDTO.ExpenseDescription;
            //expense.ModifiedBy = expenseDTO.ModifiedBy;
            expense.LastModifiedOn = DateTime.UtcNow;

            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return ApiResponse<Expense>.SuccessResponse(expense, "Update Successfull");
        }

        public async Task<ApiResponse<bool>> DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return ApiResponse<bool>.FailureResponse("Expense not found");
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Delete successfully");
        }

        public async Task<ApiResponse<List<Expense>>> GetAllExpensesAsync()
        {
            var expenses = await _context.Expenses.ToListAsync();
            return expenses != null ? ApiResponse<List<Expense>>.SuccessResponse(expenses) : ApiResponse<List<Expense>>.FailureResponse("Failes to get List");
        }
        public async Task<List<Expense>> GetExpenseAsync(Guid userId)
        {
            var result = await _context.Expenses
                .Where(e => e.DriverID == userId)
                .ToListAsync();
            return result;
            //if (result == null || result.Count == 0)
            //{
            //    return ApiResponse<List<Expense>>.FailureResponse("User Not Found/Failed to load data");
            //}

            //return ApiResponse<List<Expense>>.SuccessResponse(result);
        }
    }
}
