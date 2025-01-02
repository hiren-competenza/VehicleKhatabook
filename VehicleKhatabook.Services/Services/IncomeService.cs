using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;
using VehicleKhatabook.Repositories.Interfaces;
using VehicleKhatabook.Repositories.Repositories;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IIncomeRepository _incomeRepository;

        public IncomeService(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public async Task<UserIncome> AddIncomeAsync(IncomeDTO incomeDTO)
        {
            return await _incomeRepository.AddIncomeAsync(incomeDTO);
        }

        public async Task<ApiResponse<UserIncome>> GetIncomeDetailsAsync(int id)
        {
            return await _incomeRepository.GetIncomeDetailsAsync(id);
        }

        public async Task<UserIncome> UpdateIncomeAsync(IncomeDTO incomeDTO, int incomeExpenseId)
        {
            return await _incomeRepository.UpdateIncomeAsync(incomeDTO, incomeExpenseId);
        }

        public async Task<bool> DeleteIncomeAsync(int incomeExpenseId)
        {
            return await _incomeRepository.DeleteIncomeAsync(incomeExpenseId);
        }

        //public async Task<ApiResponse<List<Income>>> GetAllIncomesAsync()
        //{
        //    return await _incomeRepository.GetAllIncomesAsync();
        //}
        public async Task<List<UserIncome>> GetIncomeAsync(Guid vehicleId, DateTime fromDate, DateTime toDate)
        {
            return await _incomeRepository.GetIncomeAsync(vehicleId, fromDate, toDate);
        }
        public async Task<List<IncomeExpenseDTO>> GetIncomeAsync(Guid vehicleId)
        {
            return await _incomeRepository.GetIncomeAsync(vehicleId);
        }
        public async Task<List<IncomeExpenseDTO>> GetIncomebyUserAsync(Guid userId)
        {
            return await _incomeRepository.GetIncomebyUserAsync(userId);
        }
        
    }
}
