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
                Name = incomeDTO.Name,
                UserId = incomeDTO.UserId,
                Mobile = incomeDTO.Mobile,
                Date = incomeDTO.Date,
                Amount = incomeDTO.Amount,
                Note = incomeDTO.Note,
                DriverOwnerId = incomeDTO.DriverOwnerUserId,
            };
            _context.OwnerKhataCredits.Add(income);
            await _context.SaveChangesAsync();
            return income;
        }

        public async Task<List<OwnerKhataCredit>> GetOwnerIncomeAsync(Guid userId, Guid driverOwnerUserId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.OwnerKhataCredits
                .Where(e => e.UserId == userId && e.DriverOwnerId == driverOwnerUserId && e.Date >= fromDate && e.Date <= toDate)
                .ToListAsync();
            return result;
        }

        public async Task<ApiResponse<OwnerKhataCredit>> GetOwnerIncomeDetailsAsync(Guid id)
        {
            var income = await _context.OwnerKhataCredits.FindAsync(id);
            return income != null ? ApiResponse<OwnerKhataCredit>.SuccessResponse(income, "Income details retrieved successfully.") : ApiResponse<OwnerKhataCredit>.FailureResponse("Income not found");

        }
    }
}
