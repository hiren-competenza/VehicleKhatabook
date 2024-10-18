using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        Task<ApiResponse<Income>> AddIncomeAsync(IncomeDTO incomeDTO);  
        Task<ApiResponse<Income>> GetIncomeDetailsAsync(int id);
        Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO);
        Task<ApiResponse<bool>> DeleteIncomeAsync(int id);
        Task<ApiResponse<List<Income>>> GetAllIncomesAsync();
        Task<ApiResponse<List<Income>>> GetIncomeAsync(Guid userId);
    }
}
