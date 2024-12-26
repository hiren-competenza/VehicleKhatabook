using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IOwnerIncomeService
    {
        Task<OwnerKhataCredit> AddOwnerIncomeAsync(OwnerIncomeExpenseDTO incomeDTO);
        Task<ApiResponse<OwnerKhataCredit>> GetOwnerIncomeDetailsAsync(Guid id);
        Task<List<OwnerIncomeExpenseDTO>> GetOwnerIncomeAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate);
        Task<List<OwnerIncomeExpenseDTO>> GetOwnerIncomeAsync(Guid driverOwnerUserId);
        Task<List<OwnerIncomeExpenseDTO>> GetOwnerIncomebyUserAsync(Guid userId);
        Task<bool> AccountSettlementIncomeAsync(Guid driverOwnerUserId, Guid userId);
        Task<OwnerKhataCredit> UpdateOwnerIncomeAsync(Guid userId, OwnerIncomeExpenseDTO ownerDTO);
        Task<bool> DeleteOwnerIncomeAsync(Guid userId);
    }
}
