using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Repositories.Interfaces
{
    public interface IIncomeRepository
    {
        Task<UserIncome> AddIncomeAsync(IncomeDTO incomeDTO);  
        Task<ApiResponse<UserIncome>> GetIncomeDetailsAsync(int id);
        //Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO);
        //Task<ApiResponse<bool>> DeleteIncomeAsync(int id);
        //Task<ApiResponse<List<Income>>> GetAllIncomesAsync();
        Task<List<UserIncome>> GetIncomeAsync(Guid userId, Guid vehicleId, DateTime fromDate, DateTime toDate);
    }
}
