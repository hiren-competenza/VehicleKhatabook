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
                //UserID = expenseDTO.UserId,
                ExpenseVehicleId = expenseDTO.ExpenseVehicleId,
                //CreatedBy = expenseDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.UserExpenses.Add(expense);
            await _context.SaveChangesAsync();
            await _context.Entry(expense).Reference(v => v.ExpenseCategory).LoadAsync();
            await _context.Entry(expense).Reference(v => v.Vehicle).LoadAsync();
            if (expense.Vehicle != null)
            {
                await _context.Entry(expense.Vehicle).Reference(v => v.User).LoadAsync();
            }
            return expense;
        }

        public async Task<ApiResponse<UserExpense>> GetExpenseDetailsAsync(int id)
        {
            var expense = await _context.UserExpenses.FindAsync(id);
            return expense != null ? ApiResponse<UserExpense>.SuccessResponse(expense, "Expense details retrieved successfully.") : ApiResponse<UserExpense>.FailureResponse("Expense not found");
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
            expense.ExpenseVehicleId = expenseDTO.ExpenseVehicleId;
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
        public async Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.UserExpenses
                .Where(e => e.ExpenseVehicleId == vehicleId && e.ExpenseDate >= fromDate && e.ExpenseDate <= toDate)
                .Include(i => i.ExpenseCategory)
                //.Include(i => i.Vehicle)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.ExpenseDate)
                .ToListAsync();
            return result;
        }

        public async Task<List<UserExpense>> GetExpenseAsync(Guid vehicleId)
        {
            var result = await _context.UserExpenses
                .Where(e => e.ExpenseVehicleId == vehicleId)
                .Include(i => i.ExpenseCategory)
                //.Include(i => i.Vehicle)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.ExpenseDate)
                .ToListAsync();
            return result;
        }
        public async Task<List<UserExpense>> GetExpensebyUserAsync(Guid userId)
        {
            var result = await _context.UserExpenses
                .Where(e => e.Vehicle.UserID == userId)
                .Include(i => i.ExpenseCategory)
                //.Include(i => i.Vehicle)
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.VehicleType) // Include VehicleType through Vehicle
                .Include(e => e.Vehicle)            // Include related Vehicle details
                    .ThenInclude(v => v.User)
                .OrderByDescending(i => i.ExpenseDate)
                .ToListAsync();
            return result;
        }
    }
}
