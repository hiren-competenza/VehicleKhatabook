using VehicleKhatabook.Entities.Models;
using VehicleKhatabook.Models.Common;
using VehicleKhatabook.Models.DTOs;

namespace VehicleKhatabook.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<UserIncome> AddIncomeAsync(IncomeDTO incomeDTO);
        Task<ApiResponse<UserIncome>> GetIncomeDetailsAsync(int id);
        //Task<ApiResponse<Income>> UpdateIncomeAsync(int id, IncomeDTO incomeDTO);
        //Task<ApiResponse<bool>> DeleteIncomeAsync(int id);
        //Task<ApiResponse<List<Income>>> GetAllIncomesAsync();
        Task<List<UserIncome>> GetIncomeAsync(Guid vehicleId, DateTime fromDate, DateTime toDate);
    }
}
