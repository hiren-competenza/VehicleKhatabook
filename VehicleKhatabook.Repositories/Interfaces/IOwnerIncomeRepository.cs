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
    public interface IOwnerIncomeRepository
    {
        Task<OwnerKhataCredit> AddOwnerIncomeAsync(OwnerIncomeExpenseDTO incomeDTO);
        Task<ApiResponse<OwnerKhataCredit>> GetOwnerIncomeDetailsAsync(Guid id);
        Task<List<OwnerKhataCredit>> GetOwnerIncomeAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate);
        Task<List<OwnerKhataCredit>> GetOwnerIncomeAsync(Guid driverOwnerUserId);
        Task<List<OwnerKhataCredit>> GetOwnerIncomebyUserAsync(Guid userId);
        Task<bool> AccountSettlementIncomeAsync(Guid driverOwnerUserId, Guid userId);
    }
}
