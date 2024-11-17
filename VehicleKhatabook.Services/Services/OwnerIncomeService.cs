using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class OwnerIncomeService : IOwnerIncomeService
    {
        private readonly IOwnerIncomeRepository _ownerIncomeRepository;

        public OwnerIncomeService(IOwnerIncomeRepository ownerIncomeRepository)
        {
            _ownerIncomeRepository = ownerIncomeRepository;
        }
        public async Task<OwnerKhataCredit> AddOwnerIncomeAsync(OwnerIncomeExpenseDTO IncomeDTO)
        {
            return await _ownerIncomeRepository.AddOwnerIncomeAsync(IncomeDTO);
        }

        public async Task<List<OwnerKhataCredit>> GetOwnerIncomeAsync(Guid driverOwnerUserId, DateTime fromDate, DateTime toDate)
        {
            return await _ownerIncomeRepository.GetOwnerIncomeAsync(driverOwnerUserId, fromDate, toDate);
        }

        public async Task<ApiResponse<OwnerKhataCredit>> GetOwnerIncomeDetailsAsync(Guid id)
        {
            return await _ownerIncomeRepository.GetOwnerIncomeDetailsAsync(id);
        }
    }
}
