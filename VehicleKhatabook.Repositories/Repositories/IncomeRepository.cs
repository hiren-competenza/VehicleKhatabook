﻿using Microsoft.EntityFrameworkCore;
using VehicleKhatabook.Entities;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;

namespace VehicleKhatabook.Repositories.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly VehicleKhatabookDbContext _context;

        public IncomeRepository(VehicleKhatabookDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Income>> AddIncomeAsync(IncomeDTO incomeDTO)
        {
            var income = new Income
            {
                VehicleID = incomeDTO.VehicleID,
                IncomeCategoryID = incomeDTO.IncomeCategoryID,
                IncomeSource = incomeDTO.IncomeSource,
                IncomeAmount = incomeDTO.IncomeAmount,
                IncomeDate = incomeDTO.IncomeDate,
                DriverID = incomeDTO.DriverID,
                //CreatedBy = incomeDTO.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return new ApiResponse<Income> { Success = true, Data = income };
        }

        public async Task<ApiResponse<Income>> GetIncomeDetailsAsync(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            return income != null ? new ApiResponse<Income> { Success = true, Data = income } : new ApiResponse<Income> { Success = false, Message = "Income not found" };
        }

        public async Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return new ApiResponse<Income> { Success = false, Message = "Income not found" };
            }

            income.IncomeSource = incomeDTO.IncomeSource;
            income.IncomeAmount = incomeDTO.IncomeAmount;
            income.IncomeCategoryID = incomeDTO.IncomeCategoryID;
            income.IncomeDate = incomeDTO.IncomeDate;
            //income.ModifiedBy = incomeDTO.ModifiedBy;
            income.LastModifiedOn = DateTime.UtcNow;

            _context.Incomes.Update(income);
            await _context.SaveChangesAsync();
            return new ApiResponse<Income> { Success = true, Data = income };
        }

        public async Task<ApiResponse<bool>> DeleteIncomeAsync(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return new ApiResponse<bool> { Success = false, Message = "Income not found" };
            }

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool> { Success = true, Data = true };
        }

        public async Task<ApiResponse<List<Income>>> GetAllIncomesAsync()
        {
            var incomes = await _context.Incomes.ToListAsync();
            return new ApiResponse<List<Income>> { Success = true, Data = incomes };
        }
    }
}