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
                Name = expenseDTO.Name,
                //UserId = expenseDTO.UserId,
                Mobile = expenseDTO.Mobile,
                Date = expenseDTO.Date,
                Amount = expenseDTO.Amount,
                Note = expenseDTO.Note,
                DriverOwnerId = expenseDTO.DriverOwnerUserId,
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
                .ToListAsync();
            return result;
        }

        public async Task<ApiResponse<OwnerKhataDebit>> GetOwnerExpenseDetailsAsync(Guid id)
        {
            var expense = await _context.OwnerKhataDebits.FindAsync(id);
            return expense != null ? ApiResponse<OwnerKhataDebit>.SuccessResponse(expense, "Expense details retrieved successfully.") : ApiResponse<OwnerKhataDebit>.FailureResponse("Expense not found");

        }
    }
}
