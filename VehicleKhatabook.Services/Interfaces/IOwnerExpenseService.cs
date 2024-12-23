﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IOwnerExpenseService
    {
        Task<OwnerKhataDebit> AddOwnerExpenseAsync(OwnerIncomeExpenseDTO expenseDTO);
        Task<ApiResponse<OwnerKhataDebit>> GetOwnerExpenseDetailsAsync(Guid id);
        Task<List<OwnerKhataDebit>> GetOwnerExpenseAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate);
        Task<List<OwnerKhataDebit>> GetOwnerExpenseAsync(Guid driverOwnerUserId);
        Task<List<OwnerKhataDebit>> GetOwnerExpensebyUserAsync(Guid userId);
        Task<bool> AccountSettlementExpenseAsync(Guid driverOwnerUserId, Guid userId);
        Task<OwnerKhataDebit> UpdateOwnerExpenseAsync(Guid userId, OwnerIncomeExpenseDTO ownerDTO);
        Task<bool> DeleteOwnerExpenseAsync(Guid userId);
    }
}
