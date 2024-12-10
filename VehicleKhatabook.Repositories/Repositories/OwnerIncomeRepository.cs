using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class OwnerIncomeRepository : IOwnerIncomeRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public OwnerIncomeRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }
        public async Task<OwnerKhataCredit> AddOwnerIncomeAsync(OwnerIncomeExpenseDTO incomeDTO)
        {
            var income = new OwnerKhataCredit
            {
                //Name = incomeDTO.Name,
                //UserId = incomeDTO.UserId,
                //Mobile = incomeDTO.Mobile,
                Date = incomeDTO.Date,
                Amount = incomeDTO.Amount,
                Note = incomeDTO.Note,
                DriverOwnerId = incomeDTO.DriverOwnerUserId,
            };
            _context.OwnerKhataCredits.Add(income);
            await _context.SaveChangesAsync();
            await _context.Entry(income).Reference(i => i.DriverOwnerUser).LoadAsync();

            // Load Vehicle's related User details
            if (income.DriverOwnerUser != null)
            {
                await _context.Entry(income.DriverOwnerUser).Reference(v => v.user).LoadAsync();
            }
            return income;
        }

        public async Task<List<OwnerKhataCredit>> GetOwnerIncomeAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.OwnerKhataCredits
                .Where(e => e.DriverOwnerId == driverOwnerUserId && e.Date >= fromDate && e.Date <= toDate)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user)
                .OrderByDescending(i => i.Date)
                .ToListAsync();
            return result;
        }

        public async Task<List<OwnerKhataCredit>> GetOwnerIncomeAsync(Guid driverOwnerUserId)
        {
            var result = await _context.OwnerKhataCredits
                .Where(e => e.DriverOwnerId == driverOwnerUserId)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user)
                .OrderByDescending(i => i.Date)
                .ToListAsync();
            return result;
        }

        public async Task<List<OwnerKhataCredit>> GetOwnerIncomebyUserAsync(Guid userId)
        {
            var result = await _context.OwnerKhataCredits
                .Where(e => e.DriverOwnerUser.UserID == userId)
                .Include(e => e.DriverOwnerUser)            // Include related Vehicle details
                    .ThenInclude(v => v.user)
                .OrderByDescending(i => i.Date)
                .ToListAsync();
            return result;
        }

        public async Task<ApiResponse<OwnerKhataCredit>> GetOwnerIncomeDetailsAsync(Guid id)
        {
            var income = await _context.OwnerKhataCredits.FindAsync(id);
            return income != null ? ApiResponse<OwnerKhataCredit>.SuccessResponse(income, "Income details retrieved successfully.") : ApiResponse<OwnerKhataCredit>.FailureResponse("Income not found");

        }

        public async Task<bool> AccountSettlementIncomeAsync(Guid driverOwnerUserId, Guid userId)
        {
            var dataToDelete = _context.OwnerKhataCredits.Where(debit => debit.DriverOwnerId == driverOwnerUserId && debit.DriverOwnerUser.UserID == userId);
            // Remove all records that match the condition
            if (!dataToDelete.Any())
            {
                return false; // No data found to delete
            }
            _context.OwnerKhataCredits.RemoveRange(dataToDelete);
            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
