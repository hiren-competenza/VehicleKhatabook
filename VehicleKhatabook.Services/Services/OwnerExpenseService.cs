﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class OwnerExpenseService : IOwnerExpenseService
    {
        private readonly IOwnerExpenseRepository _ownerExpenseRepository;

        public OwnerExpenseService(IOwnerExpenseRepository ownerExpenseRepository)
        {
            _ownerExpenseRepository = ownerExpenseRepository;
        }
        public async Task<OwnerKhataDebit> AddOwnerExpenseAsync(OwnerIncomeExpenseDTO expenseDTO)
        {
            return await _ownerExpenseRepository.AddOwnerExpenseAsync(expenseDTO);
        }

        public async Task<List<OwnerKhataDebit>> GetOwnerExpenseAsync(Guid userId, DateTime fromDate, DateTime toDate)
        {
            return await _ownerExpenseRepository.GetOwnerExpenseAsync(userId, fromDate, toDate);
        }

        public async Task<ApiResponse<OwnerKhataDebit>> GetOwnerExpenseDetailsAsync(Guid id)
        {
            return await _ownerExpenseRepository.GetOwnerExpenseDetailsAsync(id);
        }
    }
}