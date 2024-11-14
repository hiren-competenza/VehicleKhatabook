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
        Task<List<OwnerKhataDebit>> GetOwnerExpenseAsync(Guid userId, DateTime fromDate, DateTime toDate);
    }
}
