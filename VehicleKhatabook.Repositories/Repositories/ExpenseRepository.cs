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

        public async Task<UserExpense> AddExpenseAsync(ExpenseDTO expenseDTO)
        {
            var expense = new UserExpense
            {
                ExpenseCategoryID = expenseDTO.ExpenseCategoryID,
                ExpenseAmount = expenseDTO.ExpenseAmount,
                ExpenseDate = expenseDTO.ExpenseDate,
                ExpenseDescription = expenseDTO.ExpenseDescription,
                UserID = expenseDTO.DriverID,
                //CreatedBy = expenseDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.UserExpenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<ApiResponse<UserExpense>> GetExpenseDetailsAsync(int id)
        {
            var expense = await _context.UserExpenses.FindAsync(id);
            return expense != null ? ApiResponse<UserExpense>.SuccessResponse(expense, "Expense details retrieved successfully.")  : ApiResponse<UserExpense>.FailureResponse("Expense not found");
        }

        public async Task<ApiResponse<UserExpense>> UpdateExpenseAsync(int id, ExpenseDTO expenseDTO)
        {
            var expense = await _context.UserExpenses.FindAsync(id);
            if (expense == null)
            {
                return ApiResponse<UserExpense>.FailureResponse($"Unable to update/expense for for id{id} Not Found");
            }

            expense.ExpenseAmount = expenseDTO.ExpenseAmount;
            expense.ExpenseCategoryID = expenseDTO.ExpenseCategoryID;
            expense.ExpenseDate = expenseDTO.ExpenseDate;
            expense.ExpenseDescription = expenseDTO.ExpenseDescription;
            //expense.ModifiedBy = expenseDTO.ModifiedBy;
            expense.LastModifiedOn = DateTime.UtcNow;

            _context.UserExpenses.Update(expense);
            await _context.SaveChangesAsync();
            return ApiResponse<UserExpense>.SuccessResponse(expense, "Update Successfull");
        }

        public async Task<ApiResponse<bool>> DeleteExpenseAsync(int id)
        {
            var expense = await _context.UserExpenses.FindAsync(id);
            if (expense == null)
            {
                return ApiResponse<bool>.FailureResponse("Expense not found");
            }

            _context.UserExpenses.Remove(expense);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Delete successfully");
        }

        public async Task<ApiResponse<List<UserExpense>>> GetAllExpensesAsync()
        {
            var expenses = await _context.UserExpenses.ToListAsync();
            return expenses != null ? ApiResponse<List<UserExpense>>.SuccessResponse(expenses) : ApiResponse<List<UserExpense>>.FailureResponse("Failes to get List");
        }
        public async Task<List<UserExpense>> GetExpenseAsync(Guid userId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.UserExpenses
                .Where(e => e.UserID == userId && e.ExpenseDate >= fromDate && e.ExpenseDate <= toDate)
                .Include(i => i.ExpenseCategory)
                .ToListAsync();
            return result;
        }
    }
}
