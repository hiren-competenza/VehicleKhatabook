using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IOwnerExpenseRepository
    {
        Task<OwnerKhataDebit> AddOwnerExpenseAsync(OwnerIncomeExpenseDTO expenseDTO);
        Task<ApiResponse<OwnerKhataDebit>> GetOwnerExpenseDetailsAsync(Guid id);
        Task<List<OwnerIncomeExpenseDTO>> GetOwnerExpenseAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate);
        Task<List<OwnerIncomeExpenseDTO>> GetOwnerExpenseAsync(Guid driverOwnerUserId);
        Task<List<OwnerIncomeExpenseDTO>> GetOwnerExpensebyUserAsync(Guid userId);
        Task<bool> AccountSettlementExpenseAsync(Guid driverOwnerUserId, Guid userId);
        Task<OwnerKhataDebit> UpdateOwnerExpenseAsync(Guid userId, OwnerIncomeExpenseDTO ownerDTO);
        Task<bool> DeleteOwnerExpenseAsync(Guid userId);
    }
}
