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
    public interface IOwnerExpenseService
    {
        Task<OwnerKhataDebit> AddOwnerExpenseAsync(OwnerIncomeExpenseDTO expenseDTO);
        Task<ApiResponse<OwnerKhataDebit>> GetOwnerExpenseDetailsAsync(Guid id);
        Task<List<OwnerKhataDebit>> GetOwnerExpenseAsync(Guid userId, Guid driverOwnerUserId, DateTime fromDate, DateTime toDate);
    }
}
