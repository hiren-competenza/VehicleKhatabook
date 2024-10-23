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

        public async Task<ApiResponse<Income>> AddIncomeAsync(IncomeDTO incomeDTO)
        {
            return await _incomeRepository.AddIncomeAsync(incomeDTO);
        }

        public async Task<ApiResponse<Income>> GetIncomeDetailsAsync(int id)
        {
            return await _incomeRepository.GetIncomeDetailsAsync(id);
        }

        //public async Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO)
        //{
        //    return await _incomeRepository.UpdateIncomeAsync(id, incomeDTO);
        //}

        //public async Task<ApiResponse<bool>> DeleteIncomeAsync(int id)
        //{
        //    return await _incomeRepository.DeleteIncomeAsync(id);
        //}

        //public async Task<ApiResponse<List<Income>>> GetAllIncomesAsync()
        //{
        //    return await _incomeRepository.GetAllIncomesAsync();
        //}
        public async Task<ApiResponse<List<Income>>> GetIncomeAsync(Guid userId)
        {
            return await _incomeRepository.GetIncomeAsync(userId);
        }
    }
}
