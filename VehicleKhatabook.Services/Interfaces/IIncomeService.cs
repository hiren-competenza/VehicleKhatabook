using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<Income> AddIncomeAsync(IncomeDTO incomeDTO);
        Task<ApiResponse<Income>> GetIncomeDetailsAsync(int id);
        //Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO);
        //Task<ApiResponse<bool>> DeleteIncomeAsync(int id);
        //Task<ApiResponse<List<Income>>> GetAllIncomesAsync();
        Task<List<Income>> GetIncomeAsync(Guid userId);
    }
}
