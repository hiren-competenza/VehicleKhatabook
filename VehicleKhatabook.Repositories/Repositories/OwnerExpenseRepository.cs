using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class OwnerExpenseRepository : IOwnerExpenseRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public OwnerExpenseRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }
        public async Task<OwnerKhataDebit> AddOwnerExpenseAsync(OwnerIncomeExpenseDTO expenseDTO)
        {
            var expense = new OwnerKhataDebit
            {
                //Name = expenseDTO.Name,
                //UserId = expenseDTO.UserId,
                //Mobile = expenseDTO.Mobile,
                Date = expenseDTO.Date,
                Amount = expenseDTO.Amount,
                Note = expenseDTO.Note,
                DriverOwnerId = expenseDTO.DriverOwnerUserId,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow
            };
            _context.OwnerKhataDebits.Add(expense);
            await _context.SaveChangesAsync();
            await _context.Entry(expense).Reference(i => i.DriverOwnerUser).LoadAsync();

            // Load Vehicle's related User details
            if (expense.DriverOwnerUser != null)
            {
                await _context.Entry(expense.DriverOwnerUser).Reference(v => v.user).LoadAsync();
            }
            return expense;
        }

        public async Task<List<OwnerKhataDebit>> GetOwnerExpenseAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.OwnerKhataDebits
                .Where(e => e.DriverOwnerId == driverOwnerUserId && e.Date >= fromDate && e.Date <= toDate)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user) // Include VehicleType through Vehicle
                .OrderByDescending(i => i.Date)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            return result;
        }

        public async Task<List<OwnerKhataDebit>> GetOwnerExpenseAsync(Guid driverOwnerUserId)
        {
            var result = await _context.OwnerKhataDebits
                .Where(e => e.DriverOwnerId == driverOwnerUserId)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user) // Include VehicleType through Vehicle
                .OrderByDescending(i => i.Date)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            return result;
        }

        public async Task<List<OwnerKhataDebit>> GetOwnerExpensebyUserAsync(Guid userId)
        {
            var result = await _context.OwnerKhataDebits
                .Where(e => e.DriverOwnerUser.UserID == userId)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user) // Include VehicleType through Vehicle
                .OrderByDescending(i => i.Date)
                .ThenByDescending(i => i.CreatedOn)
                .ToListAsync();
            return result;
        }

        public async Task<ApiResponse<OwnerKhataDebit>> GetOwnerExpenseDetailsAsync(Guid id)
        {
            var expense = await _context.OwnerKhataDebits.FindAsync(id);
            return expense != null ? ApiResponse<OwnerKhataDebit>.SuccessResponse(expense, "Expense details retrieved successfully.") : ApiResponse<OwnerKhataDebit>.FailureResponse("Expense not found");

        }

        public async Task<bool> AccountSettlementExpenseAsync(Guid driverOwnerUserId, Guid userId)
        {
            var dataToDelete = _context.OwnerKhataDebits.Where(debit => debit.DriverOwnerId == driverOwnerUserId && debit.DriverOwnerUser.UserID == userId);
            // Remove all records that match the condition
            if (!dataToDelete.Any())
            {
                return true; // No data found to delete
            }
            _context.OwnerKhataDebits.RemoveRange(dataToDelete);
            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OwnerKhataDebit> UpdateOwnerExpenseAsync(Guid userId, OwnerIncomeExpenseDTO ownerDTO)
        {
            var expense = await _context.OwnerKhataDebits
                .FirstOrDefaultAsync(e => e.Id == userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Expense record not found.");
            }

            expense.Amount = ownerDTO.Amount;
            expense.Note = ownerDTO.Note;

            _context.OwnerKhataDebits.Update(expense);
            await _context.SaveChangesAsync();

            if (expense.DriverOwnerUser != null)
            {
                await _context.Entry(expense.DriverOwnerUser).Reference(v => v.user).LoadAsync();
            }
            return expense;
        }

        public async Task<bool> DeleteOwnerExpenseAsync(Guid userId)
        {
            var expense = await _context.OwnerKhataDebits
                .FirstOrDefaultAsync(e => e.Id == userId);

            if (expense == null)
            {
                return false;
            }

            _context.OwnerKhataDebits.Remove(expense);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
